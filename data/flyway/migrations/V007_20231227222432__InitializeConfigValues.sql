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
              AND t.[name] = 'ConfigValue'
    )
    BEGIN
        CREATE TABLE dbo.ConfigValue
        (
            ConfigValueId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_ConfigValue_ConfigValueId PRIMARY KEY,
            ConfigDefinitionId INT NOT NULL
                CONSTRAINT FK_ConfigValue_ConfigDefinitionId_ConfigDefinition_ConfigDefinitionId
                REFERENCES dbo.ConfigDefinition (ConfigDefinitionId),
            EnvironmentId INT NULL
                CONSTRAINT FK_ConfigValue_EnvironmentId_Environment_EnvironmentId
                REFERENCES dbo.Environment (EnvironmentId),
            ApplicationId INT NULL
                CONSTRAINT FK_ConfigValue_ApplicationId_Application_ApplicationId
                REFERENCES dbo.[Application] (ApplicationId),
            ContextId INT NULL
                CONSTRAINT FK_ConfigValue_ContextId_Context_ContextId
                REFERENCES dbo.Context (ContextId),
            ConfigProviderId INT NULL
                CONSTRAINT FK_ConfigValue_ConfigProviderId_ConfigProvider_ConfigProviderId
                REFERENCES dbo.ConfigProvider (ConfigProviderId),
            ConfigValue NVARCHAR(4000) NULL,
            CreatedByGuid UNIQUEIDENTIFIER NOT NULL,
            DateCreated DATETIME2 NOT NULL
                CONSTRAINT DF_ConfigValue_DateCreated
                    DEFAULT SYSUTCDATETIME(),
            ModifiedByGuid UNIQUEIDENTIFIER NULL,
            DateModified DATETIME2 NULL
                CONSTRAINT UQ_ConfigValue_ConfigDefinitionId_EnvironmentId_ApplicationId_ContextId_ConfigProviderId
                UNIQUE NONCLUSTERED (
                                        ConfigDefinitionId,
                                        EnvironmentId,
                                        ApplicationId,
                                        ContextId,
                                        ConfigProviderId
                                    )
        )
        WITH (DATA_COMPRESSION = PAGE);
    END;

    DECLARE @UserGuid UNIQUEIDENTIFIER = 'F27E55C3-4DA5-4C38-AFED-CD7634D21A9B';

    IF NOT EXISTS
    (
        SELECT TOP (1)
               cv.ConfigValueId
        FROM dbo.ConfigValue cv
        WHERE cv.ConfigDefinitionId =
        (
            SELECT TOP (1)
                   cd.ConfigDefinitionId
            FROM dbo.ConfigDefinition cd
            WHERE cd.ConfigDefinitionGuid = 'F07D7437-27BD-4952-8696-AF18320B212E'
        )
              AND cv.EnvironmentId IS NULL
              AND cv.ApplicationId IS NULL
              AND cv.ContextId IS NULL
              AND cv.ConfigProviderId IS NULL
    )
    BEGIN
        INSERT INTO dbo.ConfigValue
        (
            ConfigDefinitionId,
            EnvironmentId,
            ApplicationId,
            ContextId,
            ConfigProviderId,
            ConfigValue,
            CreatedByGuid,
            DateCreated,
            DateModified
        )
        VALUES
        (
            (
                SELECT TOP (1)
                       cd.ConfigDefinitionId
                FROM dbo.ConfigDefinition cd
                WHERE cd.ConfigDefinitionGuid = 'F07D7437-27BD-4952-8696-AF18320B212E'
                ORDER BY cd.ConfigDefinitionId
            ), NULL, NULL, NULL, NULL, 'https://demo.duendesoftware.com', @UserGuid, DEFAULT, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               cv.ConfigValueId
        FROM dbo.ConfigValue cv
        WHERE cv.ConfigDefinitionId =
        (
            SELECT TOP (1)
                   cd.ConfigDefinitionId
            FROM dbo.ConfigDefinition cd
            WHERE cd.ConfigDefinitionGuid = '6D9B5A02-A226-4577-9596-24A45F5C1FF0'
        )
              AND cv.EnvironmentId IS NULL
              AND cv.ApplicationId IS NULL
              AND cv.ContextId IS NULL
              AND cv.ConfigProviderId IS NULL
    )
    BEGIN
        INSERT INTO dbo.ConfigValue
        (
            ConfigDefinitionId,
            EnvironmentId,
            ApplicationId,
            ContextId,
            ConfigProviderId,
            ConfigValue,
            CreatedByGuid,
            DateCreated,
            DateModified
        )
        VALUES
        (
            (
                SELECT TOP (1)
                       cd.ConfigDefinitionId
                FROM dbo.ConfigDefinition cd
                WHERE cd.ConfigDefinitionGuid = '6D9B5A02-A226-4577-9596-24A45F5C1FF0'
                ORDER BY cd.ConfigDefinitionId
            ), NULL, NULL, NULL, NULL, 'api', @UserGuid, DEFAULT, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               cv.ConfigValueId
        FROM dbo.ConfigValue cv
        WHERE cv.ConfigDefinitionId =
        (
            SELECT TOP (1)
                   cd.ConfigDefinitionId
            FROM dbo.ConfigDefinition cd
            WHERE cd.ConfigDefinitionGuid = '33427DAF-000C-4E5E-B425-A8C482A07BAC'
        )
              AND cv.EnvironmentId IS NULL
              AND cv.ApplicationId IS NULL
              AND cv.ContextId IS NULL
              AND cv.ConfigProviderId IS NULL
    )
    BEGIN
        INSERT INTO dbo.ConfigValue
        (
            ConfigDefinitionId,
            EnvironmentId,
            ApplicationId,
            ContextId,
            ConfigProviderId,
            ConfigValue,
            CreatedByGuid,
            DateCreated,
            DateModified
        )
        VALUES
        (
            (
                SELECT TOP (1)
                       cd.ConfigDefinitionId
                FROM dbo.ConfigDefinition cd
                WHERE cd.ConfigDefinitionGuid = '33427DAF-000C-4E5E-B425-A8C482A07BAC0'
                ORDER BY cd.ConfigDefinitionId
            ), NULL, NULL, NULL, NULL, 'email', @UserGuid, DEFAULT, NULL);
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;
