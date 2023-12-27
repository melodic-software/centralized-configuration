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
              AND t.[name] = 'ConfigDefinition'
    )
    BEGIN
        CREATE TABLE dbo.ConfigDefinition
        (
            ConfigDefinitionId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_ConfigDefinition_ConfigDefinitionId PRIMARY KEY,
            ConfigDefinitionGuid UNIQUEIDENTIFIER NOT NULL
                CONSTRAINT UQ_ConfigDefinition_ConfigDefinitionGuid
                UNIQUE NONCLUSTERED,
            ConfigTypeId INT NOT NULL
                CONSTRAINT FK_ConfigDefinition_ConfigTypeId_ConfigType_ConfigTypeId
                REFERENCES dbo.ConfigType (ConfigTypeId),
            KeyName NVARCHAR(150) NOT NULL
                CONSTRAINT UQ_ConfigDefinition_KeyName
                UNIQUE NONCLUSTERED,
            ConfigDefinitionDescription NVARCHAR(400) NULL,
            IsDeleted BIT NOT NULL
                CONSTRAINT DF_ConfigDefinition_IsDeleted
                    DEFAULT 0,
            CreatedByGuid UNIQUEIDENTIFIER NOT NULL,
            DateCreated DATETIME2 NOT NULL
                CONSTRAINT DF_ConfigDefinition_DateCreated
                    DEFAULT SYSUTCDATETIME(),
            ModifiedByGuid UNIQUEIDENTIFIER NULL,
            DateModified DATETIME2 NULL,
        )
        WITH (DATA_COMPRESSION = PAGE);
    END;

    DECLARE @UserGuid UNIQUEIDENTIFIER = 'F27E55C3-4DA5-4C38-AFED-CD7634D21A9B';
    DECLARE @ApplicationSettingConfigTypeId INT =
            (
                SELECT TOP (1)
                       ct.ConfigTypeId
                FROM dbo.ConfigType ct
                WHERE ct.ConfigTypeName = 'ApplicationSetting'
            );

    IF NOT EXISTS
    (
        SELECT TOP (1)
               cd.ConfigDefinitionId
        FROM dbo.ConfigDefinition cd
        WHERE cd.ConfigDefinitionGuid = 'F07D7437-27BD-4952-8696-AF18320B212E'
    )
    BEGIN
        INSERT INTO dbo.ConfigDefinition
        (
            ConfigDefinitionGuid,
            ConfigTypeId,
            KeyName,
            ConfigDefinitionDescription,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('F07D7437-27BD-4952-8696-AF18320B212E', @ApplicationSettingConfigTypeId, N'JwtBearerAuthentication:Authority',
         'The trusted authority for JWT bearer tokens.', DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               cd.ConfigDefinitionId
        FROM dbo.ConfigDefinition cd
        WHERE cd.ConfigDefinitionGuid = '6D9B5A02-A226-4577-9596-24A45F5C1FF0'
    )
    BEGIN
        INSERT INTO dbo.ConfigDefinition
        (
            ConfigDefinitionGuid,
            ConfigTypeId,
            KeyName,
            ConfigDefinitionDescription,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('6D9B5A02-A226-4577-9596-24A45F5C1FF0', @ApplicationSettingConfigTypeId, N'JwtBearerAuthentication:Audience',
         'The audience is a token claim that contains the name of the application it is meant for.', DEFAULT,
         @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               cd.ConfigDefinitionId
        FROM dbo.ConfigDefinition cd
        WHERE cd.ConfigDefinitionGuid = '33427DAF-000C-4E5E-B425-A8C482A07BAC'
    )
    BEGIN
        INSERT INTO dbo.ConfigDefinition
        (
            ConfigDefinitionGuid,
            ConfigTypeId,
            KeyName,
            ConfigDefinitionDescription,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('33427DAF-000C-4E5E-B425-A8C482A07BAC', @ApplicationSettingConfigTypeId,
         N'JwtBearerAuthentication:NameClaimType', 'The claim used for the name of the logged in user.', DEFAULT,
         @UserGuid, DEFAULT, NULL, NULL);
    END;

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;