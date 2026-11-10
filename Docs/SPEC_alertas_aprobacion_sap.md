# Servicio de alertas de aprobación SAP B1 → correo

## Objetivo

Construir un servicio en este backend (C#) que cada 5–10 minutos revise las solicitudes de aprobación pendientes en SAP Business One (HANA) y avise por correo:

- Al **aprobador** cuando una solicitud llega a su etapa.
- Al **solicitante** cuando su solicitud se aprueba, se rechaza o se cancela.
- Al **siguiente aprobador** cuando la solicitud avanza de etapa.

El servicio es **solo de notificación**: la aprobación se sigue haciendo dentro de SAP. No escribe nada en SAP.

## Antes de escribir código

Este backend ya tiene webhooks que leen SAP (y NEXO) y mandan alertas a Google Chat. **Revisa primero ese código** y reutiliza sus patrones:

- Cómo se conecta a SAP HANA (connection string, cliente, helpers de consulta).
- Cómo se programan las tareas periódicas (BackgroundService, Hangfire, Quartz, cron externo, etc.).
- Configuración (appsettings, secretos), logging e inyección de dependencias.
- Si ya existe envío de correo (SMTP, Gmail API, SendGrid, etc.). **Si no existe, pregunta** qué usar antes de elegir uno.

Propón un plan breve con los archivos que vas a crear o modificar antes de implementar.

## Reglas del negocio (validadas contra los datos reales)

1. **Pendiente** = `OWDD.Status = 'W'`.
2. **Los aprobadores salen de `WDD1`**, no de `OWST`/`WST1`. SAP crea las filas de `WDD1` cuando la solicitud llega a cada etapa.
3. **Filtrar `WDD1.StepCode = OWDD.CurrStep` y `WDD1.Status = 'W'`.** En etapas ya superadas quedan aprobadores en `'W'` para siempre: si la etapa pedía 1 aprobación y la dio otro, el resto se queda en `'W'`. Sin ese filtro se avisaría a gente de etapas pasadas.
4. **Una etapa puede tener varios aprobadores.** Se manda un aviso a cada uno.
5. **El folio a mostrar es `OWDD.DraftEntry`.** `ODRF.DocNum` en borradores es un número tentativo que se repite entre solicitudes: no usarlo.
6. **Monto:** las solicitudes de traslado (`ObjType = '1250000001'`) casi siempre traen `DocTotal = 0`. Para ese tipo no mostrar monto (idealmente almacén origen/destino; consultar las líneas del borrador en `DRF1` o el encabezado de `ODRF`, validar columnas antes).
7. **Usuarios sin correo:** algunos `OUSR.E_Mail` vienen `NULL`. No fallar: registrar el caso como error en la tabla de control y seguir.
8. **No usar ventana de 24 h.** Consultar todas las pendientes y deduplicar contra la tabla de control, para no perder nada si el job se cae.
9. **Primera ejecución:** hoy hay ~54 solicitudes en `'W'`, algunas desde 2024. Antes de activar los envíos, cargar las pendientes actuales en la tabla de control como `Enviado = 1` sin mandar correo (un modo "seed" o un flag de config).

Estados de `OWDD.Status`: `W` pendiente, `Y` aprobada, `N` rechazada, `P` generada, `A` generada por autorizador, `C` cancelada.

Tipos de documento vistos en la base: `22` orden de compra, `17` pedido de cliente, `13` factura de clientes, `1250000001` solicitud de traslado, `59` entrada de mercancías, `60` salida de mercancías.

## Consulta 1: pendientes por aprobador (probada en la base)

Devuelve una fila por aprobador de la etapa actual.

```sql
SELECT
    T0."WddCode", T0."CurrStep", S."Name" AS "Etapa",
    T1."UserID" AS "AprobadorID", UA."USER_CODE" AS "Aprobador",
    UA."U_NAME" AS "AprobadorNombre", UA."E_Mail" AS "AprobadorEmail",
    T0."ObjType",
    CASE T0."ObjType"
        WHEN '22' THEN 'Orden de compra'
        WHEN '17' THEN 'Pedido de cliente'
        WHEN '13' THEN 'Factura de clientes'
        WHEN '1250000001' THEN 'Solicitud de traslado'
        WHEN '59' THEN 'Entrada de mercancías'
        WHEN '60' THEN 'Salida de mercancías'
        ELSE T0."ObjType"
    END AS "TipoDoc",
    T0."DraftEntry" AS "Folio",
    D."DocDate", D."CardCode", D."CardName", D."DocTotal", D."DocCur",
    T0."OwnerID" AS "SolicitanteID", UO."U_NAME" AS "Solicitante",
    UO."E_Mail" AS "SolicitanteEmail",
    T0."Remarks", T0."CreateDate", T0."CreateTime"
FROM "OWDD" T0
INNER JOIN "WDD1" T1
        ON T1."WddCode" = T0."WddCode"
       AND T1."StepCode" = T0."CurrStep"
       AND T1."Status" = 'W'
LEFT JOIN "OUSR" UA ON UA."USERID" = T1."UserID"
LEFT JOIN "OUSR" UO ON UO."USERID" = T0."OwnerID"
LEFT JOIN "OWST" S  ON S."WstCode" = T0."CurrStep"
LEFT JOIN "ODRF" D  ON D."DocEntry" = T0."DraftEntry"
WHERE T0."Status" = 'W'
ORDER BY T0."WddCode", T1."SortId";
```

## Consulta 2: estado actual de solicitudes ya avisadas (sin probar)

El servicio pasa los `WddCode` abiertos de la tabla de control y compara `Status` y `CurrStep` contra lo guardado. **Probarla en HANA antes de usarla**, sobre todo los `TOP 1` en subconsultas correlacionadas. Usar parámetros, no concatenar la lista.

```sql
SELECT
    T0."WddCode", T0."Status", T0."CurrStep", S."Name" AS "Etapa",
    T0."OwnerID", UO."E_Mail" AS "SolicitanteEmail",
    (SELECT TOP 1 U2."U_NAME" FROM "WDD1" W2
       JOIN "OUSR" U2 ON U2."USERID" = W2."UserID"
      WHERE W2."WddCode" = T0."WddCode" AND W2."Status" IN ('Y','N')
      ORDER BY W2."UpdateDate" DESC, W2."UpdateTime" DESC) AS "UltimoDecisor",
    (SELECT TOP 1 W2."Remarks" FROM "WDD1" W2
      WHERE W2."WddCode" = T0."WddCode" AND W2."Status" IN ('Y','N')
      ORDER BY W2."UpdateDate" DESC, W2."UpdateTime" DESC) AS "ComentarioDecision"
FROM "OWDD" T0
LEFT JOIN "OWST" S  ON S."WstCode" = T0."CurrStep"
LEFT JOIN "OUSR" UO ON UO."USERID" = T0."OwnerID"
WHERE T0."WddCode" IN (/* parámetros */);
```

## Tabla de control (en la BD del backend, NO en SAP)

Sintaxis SQL Server; adaptar al motor y a la forma de migraciones que use este proyecto.

```sql
CREATE TABLE AprobacionAviso (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    WddCode       INT           NOT NULL,
    StepCode      INT           NOT NULL,
    UserID        INT           NOT NULL,  -- destinatario
    TipoAviso     VARCHAR(20)   NOT NULL,  -- 'PENDIENTE' | 'RESULTADO'
    StatusSap     CHAR(1)       NOT NULL,  -- estado OWDD al momento del aviso
    Email         VARCHAR(200)  NULL,
    Enviado       BIT           NOT NULL DEFAULT 0,
    Intentos      INT           NOT NULL DEFAULT 0,
    UltimoError   VARCHAR(1000) NULL,
    FechaCreacion DATETIME2     NOT NULL DEFAULT SYSDATETIME(),
    FechaEnvio    DATETIME2     NULL,
    CONSTRAINT UQ_AprobacionAviso UNIQUE (WddCode, StepCode, UserID, TipoAviso)
);
```

## Flujo de cada ejecución

1. Ejecutar la consulta 1. Por cada fila sin registro en `AprobacionAviso` (llave `WddCode + CurrStep + AprobadorID + 'PENDIENTE'`): insertar y enviar.
2. Tomar los `WddCode` con avisos `PENDIENTE` cuyo estado guardado era `W` y ejecutar la consulta 2.
   - Si `Status` cambió a `Y`, `N`, `C`, `P` o `A`: aviso `RESULTADO` al solicitante, con el decisor y el comentario.
   - Si sigue en `W` pero `CurrStep` cambió: no hace falta nada extra, porque el paso 1 detecta a los aprobadores de la nueva etapa.
3. Reintentar los registros con `Enviado = 0` hasta N intentos (configurable), guardando `UltimoError`.
4. Evitar ejecuciones solapadas, ya sea con un lock o con que el scheduler no permita concurrencia.

## Contenido del correo

- **Al aprobador:** tipo de documento, folio, fecha, solicitante, socio de negocio (si aplica), monto (si aplica), etapa y comentarios. Indicar que se aprueba en SAP B1 → Solicitudes de aprobación.
- **Al solicitante:** resultado, quién decidió, comentario y etapa.

## Decisiones pendientes (preguntar, no asumir)

- **Autoaprobación:** hay usuarios que son solicitante y aprobador de su propia solicitud en la misma etapa. ¿Se les avisa igual?
- **Enlace a SAP:** el cliente de escritorio de B1 no abre por URL. Confirmar si usan el Web Client antes de incluir un enlace.
- **Proveedor de correo y remitente.**
- **Frecuencia final:** 5 o 10 minutos.
