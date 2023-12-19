using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Web;

namespace Enterprise.Logging.Providers;

public static class NLogConfigurationService
{
    public static Logger HandleNLogPreConfiguration()
    {
        string currentDirectory = Directory.GetCurrentDirectory();

        // NOTE: if we want more control over the log output, consider creating environment named config files like "nlog.development.config"
        // the environment name isn't available until after the builder has been created, so this code would need to move to that specific event handler
        string nLogConfigFileName = "nlog.config";
        string nLogConfigFile = string.Concat(currentDirectory, $@"\{nLogConfigFileName}");

        bool fileExists = File.Exists(nLogConfigFile);

        if (!fileExists)
            throw new FileNotFoundException(nLogConfigFile);

        ISetupBuilder setupBuilder = LogManager.Setup();
        setupBuilder.LoadConfigurationFromFile(configFile: nLogConfigFile, optional: false);
        Logger? logger = setupBuilder.GetCurrentClassLogger();

        logger.Debug("PRE-CONFIGURATION");

        return logger;
    }

    public static void HandleNLogPostConfiguration(Logger? logger)
    {
        logger?.Debug("POST-CONFIGURATION");
        // ensure to flush and stop internal timers/threads before application-exit (avoid segmentation fault on Linux)
        LogManager.Shutdown();
    }

    public static void ConfigureNLog(this WebApplicationBuilder builder, bool clearExistingProviders = false)
    {
        if (clearExistingProviders)
            builder.Logging.ClearProviders();

        NLogAspNetCoreOptions options = NLogAspNetCoreOptions.Default;

        builder.Host.UseNLog(options);
    }
}