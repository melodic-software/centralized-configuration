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
            UniqueName NVARCHAR(150) NOT NULL,
            ApplicationName NVARCHAR(100) NOT NULL,
            AbbreviatedName NVARCHAR(50) NULL,
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
        WHERE a.ApplicationGuid = '500F86A2-65F7-4FC2-836A-2B14F8686209'
    )
    BEGIN
        INSERT INTO dbo.[Application]
        (
            ApplicationGuid,
            UniqueName,
            ApplicationName,
            AbbreviatedName,
            ApplicationDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('500F86A2-65F7-4FC2-836A-2B14F8686209', N'Demo Application-500f86a2', N'Demo Application', 'Demo App',
         'This is a demo application.', DEFAULT, DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               a.ApplicationId
        FROM dbo.[Application] a
        WHERE a.ApplicationGuid = 'A49262FD-9AB9-452E-92b9-BFB742C94BD0'
    )
    BEGIN
        INSERT INTO dbo.[Application]
        (
            ApplicationGuid,
            UniqueName,
            ApplicationName,
            AbbreviatedName,
            ApplicationDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('A49262FD-9AB9-452E-92b9-BFB742C94BD0', N'Demo API-a49262fd', N'Demo API', NULL, NULL, DEFAULT, DEFAULT,
         @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               a.ApplicationId
        FROM dbo.[Application] a
        WHERE a.ApplicationGuid = 'DD65FE33-97D0-4632-B7f4-677FFD2FDF14'
    )
    BEGIN
        INSERT INTO dbo.[Application]
        (
            ApplicationGuid,
            UniqueName,
            ApplicationName,
            AbbreviatedName,
            ApplicationDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('DD65FE33-97D0-4632-B7f4-677FFD2FDF14', N'Demo WinForm Application-dd65fe33', N'Demo WinForm Application',
         NULL, NULL, DEFAULT, DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;