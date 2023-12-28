USE [Configuration];
GO

CREATE OR ALTER PROCEDURE dbo.usp_CreateApplication
(
    @ApplicationGuid UNIQUEIDENTIFIER,
    @UniqueName NVARCHAR(150),
    @ApplicationName NVARCHAR(100),
    @AbbreviatedName NVARCHAR(50),
    @ApplicationDescription NVARCHAR(255),
    @IsActive BIT = 1,
    @CreatedByGuid UNIQUEIDENTIFIER,
    @ApplicationId INT OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

    BEGIN TRY
        BEGIN TRANSACTION;

        SELECT @ApplicationId = a.ApplicationId
        FROM dbo.[Application] a
        WHERE a.ApplicationGuid = @ApplicationGuid;

        IF (@ApplicationId IS NULL)
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
                DateCreated
            )
            VALUES
            (@ApplicationGuid, @UniqueName, @ApplicationName, @AbbreviatedName, @ApplicationDescription, @IsActive,
             DEFAULT, @CreatedByGuid, DEFAULT);

            SET @ApplicationId = SCOPE_IDENTITY();
        END;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
            ROLLBACK TRANSACTION;

        DECLARE @InputParams NVARCHAR(MAX)
            = CONCAT(
                        'ApplicationGuid: ',
                        CONVERT(NVARCHAR(36), @ApplicationGuid),
                        ', UniqueName: ',
                        @UniqueName,
                        ', ApplicationName: ',
                        @ApplicationName,
                        ', AbbreviatedName: ',
                        @AbbreviatedName,
                        ', ApplicationDescription: ',
                        @ApplicationDescription,
                        ', IsActive: ',
                        @IsActive,
                        ', CreatedByGuid: ',
                        CONVERT(NVARCHAR(36), @CreatedByGuid)
                    );

        INSERT INTO dbo.ErrorLog
        (
            ErrorMessage,
            ErrorSeverity,
            ErrorState,
            ProcedureName,
            AdditionalInfo
        )
        VALUES
        (   ERROR_MESSAGE(), ERROR_SEVERITY(), ERROR_STATE(),
            'dbo.usp_CreateApplication', -- Replace with dynamic SQL or leave as static if preferred
            @InputParams);

        THROW;
    END CATCH;
END;