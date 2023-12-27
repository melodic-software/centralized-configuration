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
              AND t.[name] = 'Environment'
    )
    BEGIN
        CREATE TABLE dbo.Environment
        (
            EnvironmentId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_Environment_EnvironmentId PRIMARY KEY,
            EnvironmentGuid UNIQUEIDENTIFIER NOT NULL
                CONSTRAINT UQ_Environment_EnvironmentGuid
                UNIQUE NONCLUSTERED,
            EnvironmentName NVARCHAR(50) NOT NULL,
            AbbreviatedName NVARCHAR(25) NULL,
            EnvironmentDescription NVARCHAR(255) NULL,
            IsActive BIT NOT NULL
                CONSTRAINT DF_Environment_IsActive
                    DEFAULT 0,
            IsDeleted BIT NOT NULL
                CONSTRAINT DF_Environment_IsDeleted
                    DEFAULT 0,
            CreatedByGuid UNIQUEIDENTIFIER NOT NULL,
            DateCreated DATETIME2 NOT NULL
                CONSTRAINT DF_Environment_DateCreated
                    DEFAULT SYSUTCDATETIME(),
            ModifiedByGuid UNIQUEIDENTIFIER NULL,
            DateModified DATETIME2 NULL
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
              AND t.[name] = 'EnvironmentAlias'
    )
    BEGIN
        CREATE TABLE dbo.EnvironmentAlias
        (
            EnvironmentAliasId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_EnvironmentAlias_EnvironmentAliasId PRIMARY KEY,
            EnvironmentId INT NOT NULL
                CONSTRAINT FK_EnvironmentAlias_EnvironmentId_Environment_EnvironmentId
                REFERENCES dbo.Environment (EnvironmentId),
            EnvironmentName NVARCHAR(50) NOT NULL,
            AbbreviatedName NVARCHAR(25) NULL,
            IsDeleted BIT NOT NULL
                CONSTRAINT DF_EnvironmentAlias_IsDeleted
                    DEFAULT 0,
            CreatedByGuid UNIQUEIDENTIFIER NOT NULL,
            DateCreated DATETIME2 NOT NULL
                CONSTRAINT DF_EnvironmentAlias_DateCreated
                    DEFAULT SYSUTCDATETIME(),
            ModifiedByGuid UNIQUEIDENTIFIER NULL,
            DateModified DATETIME2 NULL
                CONSTRAINT UQ_EnvironmentAlias_EnvironmentId_EnvironmentName
                UNIQUE NONCLUSTERED (
                                        EnvironmentId,
                                        EnvironmentName
                                    )
        )
        WITH (DATA_COMPRESSION = PAGE);
    END;

    DECLARE @UserGuid UNIQUEIDENTIFIER = 'F27E55C3-4DA5-4C38-AFED-CD7634D21A9B';

    IF NOT EXISTS
    (
        SELECT TOP (1)
               e.EnvironmentId
        FROM dbo.Environment e
        WHERE e.EnvironmentGuid = 'D36242CE-DB2F-4943-BC79-F7DFC05CAF06'
    )
    BEGIN
        INSERT INTO dbo.Environment
        (
            EnvironmentGuid,
            EnvironmentName,
            AbbreviatedName,
            EnvironmentDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('D36242CE-DB2F-4943-BC79-F7DFC05CAF06', N'Local', NULL,
         N'Used by individual developers for local development and testing. This environment runs on the developer''s machine and is not shared with the team.',
         1  , DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               e.EnvironmentId
        FROM dbo.Environment e
        WHERE e.EnvironmentGuid = '293201BC-C334-40B7-9835-0BCEDAF6B3FE'
    )
    BEGIN
        INSERT INTO dbo.Environment
        (
            EnvironmentGuid,
            EnvironmentName,
            AbbreviatedName,
            EnvironmentDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('293201BC-C334-40B7-9835-0BCEDAF6B3FE', N'Feature', 'Feat',
         N'Used for testing new features in isolation, this environment allows developers to validate and refine specific functionalities before integrating them into the main or develop branch (depending on the workflow).',
         0  , DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               e.EnvironmentId
        FROM dbo.Environment e
        WHERE e.EnvironmentGuid = 'E6032F07-CEA8-4973-99E8-5D5D5A2D23C4'
    )
    BEGIN
        INSERT INTO dbo.Environment
        (
            EnvironmentGuid,
            EnvironmentName,
            AbbreviatedName,
            EnvironmentDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('E6032F07-CEA8-4973-99E8-5D5D5A2D23C4', N'Development', 'Dev',
         N'A shared environment where developers merge their new code for integration testing. This is the first level of shared testing and is often continuously updated.',
         1  , DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               e.EnvironmentId
        FROM dbo.Environment e
        WHERE e.EnvironmentGuid = '9060718C-A4AC-42C9-8DF1-71F04A13F3A0'
    )
    BEGIN
        INSERT INTO dbo.Environment
        (
            EnvironmentGuid,
            EnvironmentName,
            AbbreviatedName,
            EnvironmentDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('9060718C-A4AC-42C9-8DF1-71F04A13F3A0', N'Quality Assurance', 'QA',
         N'Dedicated to thorough testing and quality assurance. QA environment closely replicates the production environment to ensure accurate test results.',
         1  , DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               e.EnvironmentId
        FROM dbo.Environment e
        WHERE e.EnvironmentGuid = 'CB7AF914-7BDA-4625-9C61-C5F9707162E5'
    )
    BEGIN
        INSERT INTO dbo.Environment
        (
            EnvironmentGuid,
            EnvironmentName,
            AbbreviatedName,
            EnvironmentDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('CB7AF914-7BDA-4625-9C61-C5F9707162E5', N'User Acceptance Testing', 'UAT',
         N'Used for user acceptance testing. This environment allows end-users or clients to validate the new features before they are deployed to production.',
         1  , DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               e.EnvironmentId
        FROM dbo.Environment e
        WHERE e.EnvironmentGuid = 'A555259F-0D6E-4578-9870-DA5AFA09BE3B'
    )
    BEGIN
        INSERT INTO dbo.Environment
        (
            EnvironmentGuid,
            EnvironmentName,
            AbbreviatedName,
            EnvironmentDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('A555259F-0D6E-4578-9870-DA5AFA09BE3B', N'Staging', 'Stage',
         N'A pre-production environment that mirrors the production setup. Used for final testing and to catch any last-minute issues before going live.',
         1  , DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               e.EnvironmentId
        FROM dbo.Environment e
        WHERE e.EnvironmentGuid = '9B0FBE9B-E2C3-4138-AE95-6C34FF04ED79'
    )
    BEGIN
        INSERT INTO dbo.Environment
        (
            EnvironmentGuid,
            EnvironmentName,
            AbbreviatedName,
            EnvironmentDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('9B0FBE9B-E2C3-4138-AE95-6C34FF04ED79', N'Production', 'Prod',
         N'The live environment used by end-users. This is the production server where the final, tested application runs.',
         1  , DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               e.EnvironmentId
        FROM dbo.Environment e
        WHERE e.EnvironmentGuid = 'E52FA2D1-4E7E-4670-95D5-3C4241DE6A59'
    )
    BEGIN
        INSERT INTO dbo.Environment
        (
            EnvironmentGuid,
            EnvironmentName,
            AbbreviatedName,
            EnvironmentDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('E52FA2D1-4E7E-4670-95D5-3C4241DE6A59', N'Replication', NULL,
         N'Used for replicating data and simulating production workloads for testing purposes.', 0, DEFAULT, @UserGuid,
         DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               e.EnvironmentId
        FROM dbo.Environment e
        WHERE e.EnvironmentGuid = '97DCD5A9-14F2-45DF-8A8C-276E949EB9DD'
    )
    BEGIN
        INSERT INTO dbo.Environment
        (
            EnvironmentGuid,
            EnvironmentName,
            AbbreviatedName,
            EnvironmentDescription,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        ('97DCD5A9-14F2-45DF-8A8C-276E949EB9DD', N'Training', 'Train',
         N'This environment is used for instructional activities, and training purposes.', 0, DEFAULT, @UserGuid,
         DEFAULT, NULL, NULL);
    END;

    IF NOT EXISTS
    (
        SELECT TOP (1)
               ea.EnvironmentAliasId
        FROM dbo.EnvironmentAlias ea
            JOIN dbo.Environment e
                ON e.EnvironmentId = ea.EnvironmentId
        WHERE e.EnvironmentGuid = '9060718C-A4AC-42C9-8DF1-71F04A13F3A0'
              AND ea.EnvironmentName = 'Testing'
    )
    BEGIN
        INSERT INTO dbo.EnvironmentAlias
        (
            EnvironmentId,
            EnvironmentName,
            AbbreviatedName,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        (
            (
                SELECT TOP (1)
                       e.EnvironmentId
                FROM dbo.Environment e
                WHERE e.EnvironmentGuid = '9060718C-A4AC-42C9-8DF1-71F04A13F3A0'
                ORDER BY e.EnvironmentId
            ), N'Testing', 'Test', DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;