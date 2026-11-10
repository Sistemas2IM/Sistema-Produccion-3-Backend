-- Tabla de control del servicio AlertasAprobacionSapService (ver Docs/SPEC_alertas_aprobacion_sap.md).
-- Ejecutar en la BD de NEXO (Prod3Connection) antes de habilitar el servicio.

CREATE TABLE dbo.AprobacionAviso (
    Id            INT IDENTITY(1,1) NOT NULL,
    WddCode       INT           NOT NULL,
    StepCode      INT           NOT NULL,
    UserID        INT           NOT NULL,  -- destinatario (OUSR.USERID)
    TipoAviso     VARCHAR(20)   NOT NULL,  -- 'PENDIENTE' | 'RESULTADO'
    StatusSap     CHAR(1)       NOT NULL,  -- estado OWDD al momento del aviso (se actualiza al cerrarse)
    Email         VARCHAR(200)  NULL,
    Enviado       BIT           NOT NULL CONSTRAINT DF_AprobacionAviso_Enviado DEFAULT 0,
    Intentos      INT           NOT NULL CONSTRAINT DF_AprobacionAviso_Intentos DEFAULT 0,
    UltimoError   VARCHAR(1000) NULL,
    FechaCreacion DATETIME2     NOT NULL CONSTRAINT DF_AprobacionAviso_FechaCreacion DEFAULT SYSDATETIME(),
    FechaEnvio    DATETIME2     NULL,
    CONSTRAINT PK_AprobacionAviso PRIMARY KEY (Id),
    CONSTRAINT UQ_AprobacionAviso UNIQUE (WddCode, StepCode, UserID, TipoAviso)
);

-- El servicio busca en cada ciclo los avisos aún abiertos en SAP
CREATE INDEX IX_AprobacionAviso_Abiertos ON dbo.AprobacionAviso (TipoAviso, StatusSap) INCLUDE (WddCode);
