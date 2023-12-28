USE [Configuration];
GO

CREATE OR ALTER PROCEDURE dbo.usp_GetApplicationById
(@ApplicationGuid UNIQUEIDENTIFIER)
AS
BEGIN
    SET NOCOUNT ON;
    SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

    SELECT a.ApplicationId,
           a.ApplicationGuid,
           a.ApplicationName,
           a.AbbreviatedName,
           a.ApplicationDescription,
           a.IsActive,
           a.IsDeleted,
           a.CreatedByGuid,
           a.DateCreated,
           a.ModifiedByGuid,
           a.DateModified
    FROM dbo.[Application] a
    WHERE a.ApplicationGuid = @ApplicationGuid;
END;