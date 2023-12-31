﻿namespace Configuration.API.Routing;

// if individual routes are versioned and NOT controller, the [MapToApiVersion] attribute can be used on controller action methods

public static class RouteTemplates
{
    public const string ApplicationCollections = "application-collections";
    public const string Applications = "applications";
    public const string ConfigurationChanges = "configuration-changes";
    public const string ConfigurationEntries = "configuration-entries";
    public const string Documentation = "documentation";
    public const string Environments = "environments";
    public const string EntityFrameworkTests = "entity-framework-tests";
    public const string Exceptions = "exceptions";
    public const string HttpConnectionInfo = "http-connection-info";
    public const string ScheduledConfigurationChanges = "scheduled-configuration-changes";
}