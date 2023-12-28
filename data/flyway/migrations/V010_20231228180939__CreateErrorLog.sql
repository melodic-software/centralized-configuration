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
              AND t.[name] = 'ErrorLog'
    )
    BEGIN
        CREATE TABLE dbo.ErrorLog
        (
            ErrorLogId INT IDENTITY(1, 1) NOT NULL
                CONSTRAINT PK_ErrorLog_ErrorLogId PRIMARY KEY,
            ErrorMessage NVARCHAR(4000) NOT NULL,
            ErrorSeverity INT NOT NULL,
            ErrorState INT NOT NULL,
            ErrorDateTime DATETIME2 NOT NULL
                CONSTRAINT DF_ErrorLog_ErrorDateTime
                    DEFAULT (SYSUTCDATETIME()),
            ProcedureName NVARCHAR(200) NOT NULL,
            AdditionalInfo NVARCHAR(4000) NULL
        )
        WITH (DATA_COMPRESSION = PAGE);


    END;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF (@@TRANCOUNT > 0)
        ROLLBACK TRANSACTION;
    THROW;
END CATCH;