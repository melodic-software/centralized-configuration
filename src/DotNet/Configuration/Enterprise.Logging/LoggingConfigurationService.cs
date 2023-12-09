using Enterprise.Logging.Http;
using Enterprise.Logging.Options;
using Enterprise.Logging.W3C;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Enterprise.TextEncoding.ConsoleEncoding;

namespace Enterprise.Logging;

public static class LoggingConfigurationService
{
    public static void ConfigureLogging(this WebApplicationBuilder builder, LoggingConfigurationOptions loggingConfigOptions)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging

        // completely customize logging config
        if (loggingConfigOptions.CustomConfigureLogging != null)
        {
            loggingConfigOptions.CustomConfigureLogging(builder.Logging);
            return;
        }

        // allow for unicode characters to be printed to the console
        // without this, they would be shown as the unicode "code" ex: \u0022a
        Console.OutputEncoding = Encoding.UTF8;

        ConsoleEncodingService.AllowSpecialCharacters();

        ConfigureProviders(builder);

        // request logging
        builder.Services.ConfigureHttpLogging(loggingConfigOptions);
        builder.Services.ConfigureW3CLogging(loggingConfigOptions);

        // allow for manual registration of log filters
        // this is an alternate over the appSettings.config file configuration
        loggingConfigOptions.AddLogFilters?.Invoke(builder.Logging);
    }

    public static void UseLogging(this WebApplication app, LoggingConfigurationOptions loggingConfigOptions)
    {
        app.UseHttpLogging(loggingConfigOptions);
        app.UseW3CLogging(loggingConfigOptions);

        // TODO: make this configurable based on the provider that has been registered
        //app.UseSerilogRequestLogging();
    }

    private static void ConfigureProviders(WebApplicationBuilder builder)
    {
        // https://learn.microsoft.com/en-us/dotnet/core/extensions/logging-providers
        builder.Logging.ClearProviders();

        // https://learn.microsoft.com/en-us/dotnet/core/extensions/logging-providers#built-in-logging-providers
        // https://learn.microsoft.com/en-us/dotnet/core/extensions/console-log-formatter
        builder.Logging.AddConsole()
            .AddDebug()
            .AddEventSourceLogger();

        // the event log is specific to Windows operating system
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            builder.Logging.AddEventLog();

        //AddApplicationInsightsTelemetry(builder);

        //AddTextFileTraceListener(loggingConfigOptions);

        //builder.ConfigureNLog();
        //builder.ConfigureSerilog();
    }

    private static void AddApplicationInsightsTelemetry(WebApplicationBuilder builder)
    {
        builder.Services.AddApplicationInsightsTelemetry();
    }

    private static void AddTextFileTraceListener(LoggingConfigurationOptions loggingConfigOptions)
    {
        if (string.IsNullOrWhiteSpace(loggingConfigOptions.LogFileApplicationName))
            throw new ArgumentNullException(nameof(loggingConfigOptions.LogFileApplicationName));

        // Mac: /Users/<username>/.local/share
        // Windows: C:\Users\<username>\AppData\local
        string localAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string applicationName = loggingConfigOptions.LogFileApplicationName;
        DateTime timestamp = DateTime.Now;
        string tracePath = Path.Join(localAppDataPath, $"Log_{applicationName}_{timestamp:yyyMMdd-HHmm}.txt");
        StreamWriter fileStreamWriter = File.CreateText(tracePath);
        TextWriterTraceListener textWriterTraceListener = new TextWriterTraceListener(fileStreamWriter);
        Trace.Listeners.Add(textWriterTraceListener);
        Trace.AutoFlush = true;
    }
}