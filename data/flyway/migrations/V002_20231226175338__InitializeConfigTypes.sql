USE [Configuration];
GO

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               t.object_id
        FROM sys.tables t
            JOIN sys.schemas s
                ON s.schema_id = t.schema_id
        WHERE s.[name] = 'dbo'
              AND t.[name] = 'ConfigDataType'
    )
    BEGIN
        CREATE TABLE dbo.ConfigDataType
        (
            ConfigDataTypeId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_ConfigDataType_ConfigDataTypeId PRIMARY KEY,
            ConfigDataTypeName NVARCHAR(50) NOT NULL
                CONSTRAINT UQ_ConfigDataType_ConfigDataTypeName
                UNIQUE NONCLUSTERED,
            IsDeleted BIT NOT NULL
                CONSTRAINT DF_ConfigDataType_IsDeleted
                    DEFAULT 0,
            CreatedByGuid UNIQUEIDENTIFIER NOT NULL,
            DateCreated DATETIME2 NOT NULL
                CONSTRAINT DF_ConfigDataType_DateCreated
                    DEFAULT SYSUTCDATETIME(),
            ModifiedByGuid UNIQUEIDENTIFIER NULL,
            DateModified DATETIME2 NULL,
        )
        WITH (DATA_COMPRESSION = PAGE);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               t.object_id
        FROM sys.tables t
            JOIN sys.schemas s
                ON s.schema_id = t.schema_id
        WHERE s.[name] = 'dbo'
              AND t.[name] = 'ConfigType'
    )
    BEGIN
        CREATE TABLE dbo.ConfigType
        (
            ConfigTypeId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_ConfigType_ConfigTypeId PRIMARY KEY,
            ConfigTypeName NVARCHAR(50) NOT NULL
                CONSTRAINT UQ_ConfigType_ConfigTypeName
                UNIQUE NONCLUSTERED,
            IsDeleted BIT NOT NULL
                CONSTRAINT DF_ConfigType_IsDeleted
                    DEFAULT 0,
            CreatedByGuid UNIQUEIDENTIFIER NOT NULL,
            DateCreated DATETIME2 NOT NULL
                CONSTRAINT DF_ConfigType_DateCreated
                    DEFAULT SYSUTCDATETIME(),
            ModifiedByGuid UNIQUEIDENTIFIER NULL,
            DateModified DATETIME2 NULL,
        )
        WITH (DATA_COMPRESSION = PAGE);
    END;

    DECLARE @UserGuid UNIQUEIDENTIFIER = 'F27E55C3-4DA5-4C38-AFED-CD7634D21A9B';

    IF NOT EXISTS
    (
        SELECT TOP (1)
               cdt.ConfigDataTypeId
        FROM dbo.ConfigDataType cdt
        WHERE cdt.ConfigDataTypeName = 'String'
    )
    BEGIN
        INSERT INTO dbo.ConfigDataType
        (
            ConfigDataTypeName,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        (N'String', DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               cdt.ConfigDataTypeId
        FROM dbo.ConfigDataType cdt
        WHERE cdt.ConfigDataTypeName = 'Boolean'
    )
    BEGIN
        INSERT INTO dbo.ConfigDataType
        (
            ConfigDataTypeName,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        (N'Boolean', DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               ct.ConfigTypeId
        FROM dbo.ConfigType ct
        WHERE ct.ConfigTypeName = 'ApplicationSetting'
    )
    BEGIN
        INSERT INTO dbo.ConfigType
        (
            ConfigTypeName,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        (N'ApplicationSetting', DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               ct.ConfigTypeId
        FROM dbo.ConfigType ct
        WHERE ct.ConfigTypeName = 'ConnectionString'
    )
    BEGIN
        INSERT INTO dbo.ConfigType
        (
            ConfigTypeName,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        (N'ConnectionString', DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               ct.ConfigTypeId
        FROM dbo.ConfigType ct
        WHERE ct.ConfigTypeName = 'FeatureToggle'
    )
    BEGIN
        INSERT INTO dbo.ConfigType
        (
            ConfigTypeName,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        (N'FeatureToggle', DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;