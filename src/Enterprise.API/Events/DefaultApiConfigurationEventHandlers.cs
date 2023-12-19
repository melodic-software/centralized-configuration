using Enterprise.Logging.Providers;
using NLog;

namespace Enterprise.API.Events;

public class DefaultApiConfigurationEventHandlers : ApiConfigurationEventHandlers
{
    public DefaultApiConfigurationEventHandlers()
    {
        // early init of NLog to allow startup and exception logging, before host is built
        Logger? logger = null;

        OnPreConfiguration = args =>
        {
            logger = NLogConfigurationService.HandleNLogPreConfiguration();
            return Task.CompletedTask;
        };

        OnBuilderCreated = builder =>
        {
            logger?.Debug("BUILDER CREATED");
            return Task.CompletedTask;
        };

        OnServicesConfigured = builder =>
        {
            logger?.Debug("SERVICES CONFIGURED");
            return Task.CompletedTask;
        };

        OnWebApplicationBuilt = app =>
        {
            logger?.Debug("WEB APPLICATION BUILT");
            return Task.CompletedTask;
        };

        OnRequestPipelineConfigured = builder =>
        {
            logger?.Debug("REQUEST PIPELINE CONFIGURED");
            return Task.CompletedTask;
        };

        OnConfigurationError = exception =>
        {
            // catch setup errors
            logger?.Error(exception, "STOPPED PROGRAM BECAUSE OF EXCEPTION");
            return Task.CompletedTask;
        };

        OnPostConfiguration = () =>
        {
            NLogConfigurationService.HandleNLogPostConfiguration(logger);
            return Task.CompletedTask;
        };
    }
}