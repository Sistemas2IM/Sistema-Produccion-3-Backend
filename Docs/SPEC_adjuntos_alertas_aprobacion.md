# Adjuntos del borrador en las alertas de aprobación SAP B1

Tarea en Notion: "[FEATURE] Adjuntos del borrador en las alertas de aprobación SAP B1" (Tareas del equipo), que depende de "[FEATURE] Notificaciones para aprobaciones en SAP B1".

## Objetivo

Que el correo que recibe el **aprobador** incluya como adjuntos los archivos del borrador (cotizaciones, proformas, DTE) que están guardados en el NAS, para que los abra con un clic desde Gmail, también desde el celular.

Se descartó mostrar la ruta de red como texto: Gmail no abre enlaces `file://` ni `\\servidor\carpeta`, y los usuarios no saben copiar rutas al Explorador.

## Estado actual (no romper)

El servicio de alertas está **en producción desde el 13/11/2026** y funcionando. Ya incluye:

- `AlertasAprobacionSapService` (BackgroundService, cada 5 min), `AprobacionesSapRepository` (ADO.NET contra HANA), `CorreoService` (SMTP Gmail, `ICorreoService`), `PlantillasAprobacion` (HTML) y la entidad `AprobacionAviso` (tabla de control en NEXO_DB).
- Detalle de líneas (`DRF1`), resuelto con **una sola consulta por ciclo** con los `DraftEntry` como parámetros. **Sigue el mismo patrón para los adjuntos.**
- "Número en SAP (borrador)" (`NNM1.SeriesName` + `ODRF.DocNum`) y enlace al Web Client por tipo (`UrlWebClientPorTipo`).
- Endpoint temporal `POST /api/test/correo`, registrado solo en Development.

**Antes de escribir código, revisa estos archivos** y reutiliza sus patrones: consultas parametrizadas, opciones y logging. Propón un plan breve antes de implementar.

## Datos en SAP (validados en HANA el 14/11/2026)

- El borrador tiene `ODRF.AtcEntry`, que apunta a `ATC1` con las columnas `AbsEntry`, `Line`, `trgtPath`, `FileName` y `FileExt`.
- Ruta completa: `trgtPath + "\" + FileName + "." + FileExt`.
  - Ejemplo (orden de compra 16677): `\\192.168.2.246\anexos\DTE0576.PDF` y `\\192.168.2.246\anexos\PROFORMA8377.pdf`.
- **`trgtPath` es NCLOB en HANA.** Léelo como texto (`GetString`). No lo uses en `GROUP BY`, `MIN`, `MAX`, `STRING_AGG` ni `DISTINCT`: HANA da el error *"Aggregate function doesn't support column with array/lob type"*.
- `trgtPath` puede venir con o sin `\` al final. Arma la ruta con `Path.Combine` o normalízala.
- La extensión viene con mayúsculas y minúsculas mezcladas (`PDF`, `pdf`). Respeta el nombre original al adjuntar.
- En los últimos 90 días, solo las **órdenes de compra (22)** traen adjuntos: 261 de 305, con un máximo de 3 archivos, casi todos PDF y alguna imagen JPEG. Traslados, facturas y pedidos no tienen. El código igual debe ser genérico para cualquier tipo.

Consulta (una por ciclo):

```sql
SELECT D."DocEntry", A."Line", A."trgtPath", A."FileName", A."FileExt"
FROM "ODRF" D
JOIN "ATC1" A ON A."AbsEntry" = D."AtcEntry"
WHERE D."DocEntry" IN (?, ?, ...)
ORDER BY D."DocEntry", A."Line";
```

## Comportamiento requerido

1. **Adjuntar** cada archivo leído del NAS con su nombre original (`FileName.FileExt`) y el tipo MIME según la extensión (`application/pdf`, `image/jpeg`, etc.; `application/octet-stream` si es desconocida).
2. **Sección "Adjuntos"** en el correo del aprobador, después del detalle de líneas: la lista de archivos, con el estado de cada uno:
   - Adjuntado
   - No encontrado
   - Sin acceso
   - Excede el tamaño, revísalo en SAP
   - No permitido

   Si el borrador no tiene adjuntos, la sección no se muestra.
3. **Límite de tamaño:** la suma de los archivos de un correo no puede pasar de `AlertasAprobacion:MaxAdjuntosMB` (por defecto: 15). Gmail permite 25 MB, pero la codificación base64 agrega cerca de un 33%. Adjunta en orden de `Line` hasta llenar el cupo; los que no quepan se listan como "excede el tamaño".
4. **Seguridad:** solo lee archivos cuya ruta, ya normalizada, empiece con alguna de `AlertasAprobacion:RutasAdjuntosPermitidas`. Cualquier otra se lista como "no permitido" y no se lee.
5. **Interruptor:** `AlertasAprobacion:AdjuntarArchivos` (por defecto: `true`). Con `false`, no se consultan ni se adjuntan archivos, y no se muestra la sección.
6. **Nunca bloquear el aviso:** si fallan la consulta de adjuntos, el acceso a la carpeta o la lectura de un archivo, el correo sale igual (con los motivos en la sección "Adjuntos") y el error se registra en el log. Un problema con un adjunto **no** cuenta como intento fallido del aviso y no cambia `Enviado` ni `Intentos`.
7. **Reintentos:** si el envío del correo falla por SMTP y se reintenta, vuelve a leer los archivos. No guardes los bytes en memoria entre ciclos.
8. **Varios aprobadores:** cada aprobador recibe su propio correo con los adjuntos. Lee los archivos una vez por ciclo y reutilízalos para todos los correos de ese mismo borrador, si es sencillo; si no, léelos por correo.
9. **Solo el aviso al aprobador.** El aviso de resultado al solicitante (`AvisarSolicitante`, hoy apagado) no lleva adjuntos.

Configuración nueva en `appsettings.json`:

```json
"AdjuntarArchivos": true,
"MaxAdjuntosMB": 15,
"RutasAdjuntosPermitidas": [ "\\\\192.168.2.246\\anexos" ]
```

(Dentro de la sección `AlertasAprobacion`, como el resto de las opciones.)

## Requisito de infraestructura (fuera del código)

En producción, el servicio corre con la identidad del grupo de aplicaciones de IIS, no con el usuario de Windows del desarrollador. Esa identidad necesita permiso de **lectura** en `\\192.168.2.246\anexos`:

- Con `ApplicationPoolIdentity` en un dominio, el acceso a la red se hace como la cuenta de equipo (`DOMINIO\SERVIDOR$`).
- Si no hay dominio, se configura el grupo de IIS con un usuario dedicado que tenga ese acceso.

**Al probar en local, los adjuntos van a funcionar aunque falte este permiso en el servidor**, porque el endpoint corre con el usuario del desarrollador. Por eso el mensaje de error de acceso debe ser claro en el log ("sin acceso a la ruta X"), para detectarlo después de publicar.

## Pruebas (endpoint `/api/test/correo`)

| Caso | Resultado esperado |
| --- | --- |
| Orden de compra `16677` | Llegan `DTE0576.PDF` y `PROFORMA8377.pdf` adjuntos, y la sección los lista como adjuntados |
| Ruta inexistente (dato ficticio) | El correo sale y lista el archivo como "no encontrado" |
| `MaxAdjuntosMB = 0` | El correo sale sin adjuntos y los lista como "excede el tamaño" |
| Ruta fuera de `RutasAdjuntosPermitidas` | No se lee y se lista como "no permitido" |
| Traslado `16621` o factura `16685` | Sin sección "Adjuntos"; el correo se ve igual que hoy |
| `AdjuntarArchivos = false` con `16677` | Sin adjuntos y sin sección |

El endpoint debe seguir mandando solo al `destino` indicado.

## Cuidados del proyecto

- **No modifiques** la lógica de detección de pendientes, la tabla de control ni el seed.
- `Program.cs` está en **Windows-1252**, no en UTF-8. Conserva esa codificación si lo editas.
- Se compila con el MSBuild de Visual Studio (`dotnet build` falla por las referencias COM de la DI API, error MSB4803):
  `"C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe" Sistema-Produccion-3-Backend.csproj -restore`
- Driver de HANA: `Sap.Data.Hana.Net.v8.0` con parámetros posicionales `?`. HANA no admite `TOP` ni `ORDER BY` en subconsultas correlacionadas.
- **Al probar en local:** tu `appsettings.json` apunta a SAP y NEXO_DB **de producción**. Mantén `AlertasAprobacion:Habilitado = false` en `appsettings.Development.json`, o el servicio va a mandar avisos reales a los aprobadores y escribir en la tabla de control.
- Haz el commit en una rama propia, no mezclado con `modulo_pesaje`.
- El `appsettings.json` del servidor no se actualiza con el merge: las opciones nuevas se agregan a mano.

## Después de publicar

1. Agregar las tres opciones nuevas al `appsettings.json` del servidor.
2. Esperar la primera orden de compra con adjuntos y revisar que el correo los traiga.
3. Revisar el log: si aparece "sin acceso", falta el permiso de lectura de la identidad de IIS sobre el NAS.
