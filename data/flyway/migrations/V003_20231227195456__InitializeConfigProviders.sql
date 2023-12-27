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
              AND t.[name] = 'ConfigProvider'
    )
    BEGIN
        CREATE TABLE dbo.ConfigProvider
        (
            ConfigProviderId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_ConfigProvider_ConfigProviderId PRIMARY KEY,
            ConfigProviderGuid UNIQUEIDENTIFIER NOT NULL
                CONSTRAINT UQ_ConfigProvider_ConfigProviderGuid
                UNIQUE NONCLUSTERED,
            ProviderName NVARCHAR(50) NOT NULL
                CONSTRAINT UQ_ConfigProvider_ProviderName
                UNIQUE NONCLUSTERED,
            IsDeleted BIT NOT NULL
                CONSTRAINT DF_ConfigProvider_IsDeleted
                    DEFAULT 0,
            CreatedByGuid UNIQUEIDENTIFIER NOT NULL,
            DateCreated DATETIME2 NOT NULL
                CONSTRAINT DF_ConfigProvider_DateCreated
                    DEFAULT SYSUTCDATETIME(),
            ModifiedByGuid UNIQUEIDENTIFIER NULL,
            DateModified DATETIME2 NULL
        )
        WITH (DATA_COMPRESSION = PAGE);
    END;

    DECLARE @UserGuid UNIQUEIDENTIFIER = 'F27E55C3-4DA5-4C38-AFED-CD7634D21A9B';

    IF NOT EXISTS
    (
        SELECT TOP (1)
               cp.ConfigProviderId
        FROM dbo.ConfigProvider cp
        WHERE cp.ConfigProviderGuid = '64053744-77DF-4A02-BA06-E06C041BC683'
    )
    BEGIN
        INSERT INTO dbo.ConfigProvider
        (
            ConfigProviderGuid,
            ProviderName,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('64053744-77DF-4A02-BA06-E06C041BC683', N'AzureAppConfiguration', DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

	IF NOT EXISTS
    (
        SELECT TOP (1)
               cp.ConfigProviderId
        FROM dbo.ConfigProvider cp
        WHERE cp.ConfigProviderGuid = '05F89101-BDB9-4452-9C9E-717AF6AAF385'
    )
    BEGIN
        INSERT INTO dbo.ConfigProvider
        (
            ConfigProviderGuid,
            ProviderName,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('05F89101-BDB9-4452-9C9E-717AF6AAF385', N'AzureKeyVault', DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;