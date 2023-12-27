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
              AND t.[name] = 'Context'
    )
    BEGIN
        CREATE TABLE dbo.Context
        (
            ContextId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_Context_ContextId PRIMARY KEY,
            ContextName NVARCHAR(50) NOT NULL
                CONSTRAINT UQ_Context_ContextName
                UNIQUE NONCLUSTERED,
            IsActive BIT NOT NULL
                CONSTRAINT DF_Context_IsActive_True
                    DEFAULT 1,
            IsDeleted BIT NOT NULL
                CONSTRAINT DF_Context_IsDeleted_False
                    DEFAULT 0,
            CreatedByGuid UNIQUEIDENTIFIER NOT NULL,
            DateCreated DATETIME2 NOT NULL
                CONSTRAINT DF_Context_DateCreated
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
               c.ContextId
        FROM dbo.Context c
        WHERE c.ContextName = 'kyle.sexton-6a2e51e8'
    )
    BEGIN
        INSERT INTO dbo.Context
        (
            ContextName,
            IsActive,
            IsDeleted,
            CreatedByGuid,
            DateCreated,
            ModifiedByGuid,
            DateModified
        )
        VALUES
        (N'kyle.sexton-6a2e51e8', DEFAULT, DEFAULT, @UserGuid, DEFAULT, NULL, NULL);
    END;

    COMMIT TRANSACTION;

END TRY
BEGIN CATCH
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;