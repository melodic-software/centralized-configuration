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
              AND t.[name] = 'Application'
    )
    BEGIN
        CREATE TABLE dbo.[Application]
        (
            ApplicationId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_Application_ApplicationId PRIMARY KEY,
            ApplicationGuid UNIQUEIDENTIFIER NOT NULL
                CONSTRAINT UQ_Application_ApplicationGuid
                UNIQUE NONCLUSTERED,
            ApplicationName NVARCHAR(100) NOT NULL,
            AbbreviatedName NVARCHAR(150) NULL,
            ApplicationDescription NVARCHAR(255) NULL,
            IsActive BIT NOT NULL
                CONSTRAINT DF_Application_IsActive
                    DEFAULT 0,
            IsDeleted BIT NOT NULL
                CONSTRAINT DF_Application_IsDeleted
                    DEFAULT 0,
            CreatedByGuid UNIQUEIDENTIFIER NOT NULL,
            DateCreated DATETIME2 NOT NULL
                CONSTRAINT DF_Application_DateCreated
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
               a.ApplicationId
        FROM dbo.[Application] a
        WHERE a.ApplicationGuid = '5C901FE0-3086-48B1-B4EE-A315BA2E3E91'
    )
    BEGIN
        INSERT INTO dbo.[Application]
        (
            ApplicationGuid,
            ApplicationName,
            AbbreviatedName,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('5C901FE0-3086-48B1-B4EE-A315BA2E3E91', N'DemoApplication', 'Demo Application', DEFAULT, DEFAULT, @UserGuid,
         DEFAULT, NULL, NULL);
    END;

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;