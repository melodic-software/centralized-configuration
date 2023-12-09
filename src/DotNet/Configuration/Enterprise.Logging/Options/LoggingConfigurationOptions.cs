using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Options;

public class LoggingConfigurationOptions
{
    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging
    /// HTTP Logging is a middleware that logs information about incoming HTTP requests and HTTP responses.
    /// It can reduce the performance of an app, especially when logging the request and response bodies.
    /// It can potentially log personally identifiable information (PII). Consider the risk and avoid logging sensitive information.
    /// </summary>
    public bool EnableHttpLogging { get; set; }

    /// <summary>
    /// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/w3c-logger
    /// W3CLogger is a middleware that writes log files in the W3C standard format. The logs contain information about HTTP requests and HTTP responses.
    /// It can reduce the performance of an app, especially when logging the request and response bodies.
    /// It can potentially log personally identifiable information (PII). Consider the risk and avoid logging sensitive information.
    /// </summary>
    public bool EnableW3CLogging { get; set; }

    /// <summary>
    /// The name of the application that is safe to use for file system operations.
    /// Folder names, file names, etc.
    /// </summary>
    public string LogFileApplicationName { get; set; }

    /// <summary>
    /// A custom configuration delegate for configuring log filters (in code).
    /// It is recommended to use configuration (appSettings.json) over in code configuration.
    /// </summary>
    public Action<ILoggingBuilder>? AddLogFilters { get; set; }

    /// <summary>
    /// Completely customize logging configuration.
    /// If this is provided, all of the logging configuration defaults will not be configured.
    /// </summary>
    public Action<ILoggingBuilder>? CustomConfigureLogging { get; set; }

    public LoggingConfigurationOptions()
    {
        EnableHttpLogging = false;
        EnableW3CLogging = false;
        LogFileApplicationName = string.Empty;
        AddLogFilters = null;
        CustomConfigureLogging = null;
    }
}