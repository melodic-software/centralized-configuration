USE [Configuration];
GO

CREATE OR ALTER VIEW dbo.ConfigValueList
WITH SCHEMABINDING
AS
SELECT cv.ConfigValueId,
       cv.ConfigDefinitionId,
       cd.ConfigDefinitionGuid,
       cd.ConfigTypeId,
       ct.ConfigTypeName,
       ct.IsDeleted AS ConfigTypeIsDeleted,
       ct.CreatedByGuid AS ConfigTypeCreatedByGuid,
       ct.DateCreated AS DateConfigTypeCreated,
       ct.ModifiedByGuid AS ConfigTypeModifiedByGuid,
       ct.DateModified AS DateConfigTypeModified,
       cd.KeyName AS ConfigDefinitionKeyName,
       cd.ConfigDefinitionDescription,
       cd.IsDeleted AS ConfigDefinitionIsDeleted,
       cd.CreatedByGuid AS ConfigDefinitionCreatedBy,
       cd.DateCreated AS DateConfigDefinitionCreated,
       cd.ModifiedByGuid AS ConfigDefinitionModifiedByGuid,
       cd.DateModified AS DateConfigDefinitionModified,
       cv.EnvironmentId,
       e.EnvironmentGuid,
       e.EnvironmentName,
       e.AbbreviatedName AS AbbreviatedEnvironmentName,
       e.EnvironmentDescription,
       e.IsActive AS EnvironmentIsActive,
       e.IsDeleted AS EnvironmentIsDeleted,
       e.CreatedByGuid AS EnvironmentCreatedByGuid,
       e.DateCreated AS DateEnvironmentCreated,
       e.ModifiedByGuid AS EnvironmentModifiedByGuid,
       e.DateModified AS DateEnvironmentModified,
       cv.ApplicationId,
       a.ApplicationGuid,
       a.ApplicationName,
       a.DisplayName AS ApplicationDisplayName,
       a.IsActive AS ApplicationIsActive,
       a.IsDeleted AS ApplicationIsDeleted,
       a.CreatedByGuid AS ApplicationCreatedByGuid,
       a.DateCreated AS DateApplicationCreated,
       a.ModifiedByGuid AS ApplicationModifiedByGuid,
       a.DateModified AS DateApplicationModified,
       cv.ContextId,
       ctx.ContextName,
       ctx.IsActive AS ContextIsActive,
       ctx.IsDeleted AS ContextIsDeleted,
       ctx.CreatedByGuid AS ContextCreatedByGuid,
       ctx.DateCreated AS DateContextCreated,
       ctx.ModifiedByGuid AS ContextModifiedByGuid,
       ctx.DateModified AS DateContextModified,
       cv.ConfigProviderId,
       cp.ConfigProviderGuid,
       cp.ProviderName AS ConfigProviderName,
       cp.IsDeleted AS ConfigProviderIsDeleted,
       cp.CreatedByGuid AS ConfigProviderCreatedByGuid,
       cp.DateCreated AS DateConfigProviderCreated,
       cp.ModifiedByGuid AS ConfigProviderModifiedByGuid,
       cp.DateModified AS DateConfigProviderModified,
       cv.ConfigValue,
       cv.CreatedByGuid AS ConfigValueCreatedByGuid,
       cv.DateCreated AS DateConfigValueCreated,
       cv.ModifiedByGuid AS ConfigValueModifiedByGuid,
       cv.DateModified AS DateConfigValueModified
FROM dbo.ConfigValue cv
    JOIN dbo.ConfigDefinition cd
        ON cd.ConfigDefinitionId = cv.ConfigDefinitionId
    JOIN dbo.ConfigType ct
        ON ct.ConfigTypeId = cd.ConfigTypeId
    LEFT JOIN dbo.Environment e
        ON e.EnvironmentId = cv.EnvironmentId
    LEFT JOIN dbo.[Application] a
        ON a.ApplicationId = cv.ApplicationId
    LEFT JOIN dbo.Context ctx
        ON ctx.ContextId = cv.ContextId
    LEFT JOIN dbo.ConfigProvider cp
        ON cp.ConfigProviderId = cv.ConfigProviderId;