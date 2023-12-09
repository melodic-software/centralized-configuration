USE [Configuration];
GO

SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

BEGIN TRANSACTION;

IF EXISTS
(
    SELECT p.[object_id]
    FROM [sys].[procedures] p
        JOIN [sys].[schemas] s
            ON s.[schema_id] = p.[schema_id]
    WHERE s.[name] = 'dbo'
          AND p.[name] = 'usp_ConfigurationValue_Delete'
          AND p.[type] = 'P'
)
    DROP PROCEDURE dbo.usp_ConfigurationValue_Delete;
GO

CREATE PROCEDURE dbo.usp_ConfigurationValue_Delete
(@ConfigurationValueId INT)
AS
BEGIN
    SET NOCOUNT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    DELETE FROM dbo.ConfigurationValue
    WHERE ConfigurationValueId = @ConfigurationValueId;
END;
GO

IF EXISTS
(
    SELECT p.[object_id]
    FROM [sys].[procedures] p
        JOIN [sys].[schemas] s
            ON s.[schema_id] = p.[schema_id]
    WHERE s.[name] = 'dbo'
          AND p.[name] = 'usp_ConfigurationValue_Delete'
          AND p.[type] = 'P'
)
    COMMIT TRANSACTION;
ELSE
    ROLLBACK TRANSACTION;
GO