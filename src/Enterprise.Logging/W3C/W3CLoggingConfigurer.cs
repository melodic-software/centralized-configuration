using Enterprise.Logging.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Logging.W3C;

public static class W3CLoggingConfigurer
{
    // 5,242,880 bytes = 5.24288 megabytes
    private const int FileSizeLimitInBytes = 5 * 1024 * 1024;
    private const int FlushIntervalInSeconds = 2;

    public static void ConfigureW3CLogging(this IServiceCollection services, LoggingConfigurationOptions loggingConfigOptions)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/w3c-logger

        if (string.IsNullOrWhiteSpace(loggingConfigOptions.LogFileApplicationName))
            throw new ArgumentNullException(nameof(loggingConfigOptions.LogFileApplicationName));

        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        // example directory for Windows:
        // C:\Users\{username}\AppData\Local\logs
        string logDirectoryName = "logs";
        string logDirectory = Path.Combine(appDataPath, logDirectoryName);

        // the resulting file will look like this: {ApplicationName}-W3C20230906.0000.txt
        string fileNamePrefix = $"{loggingConfigOptions.LogFileApplicationName}-W3C";

        services.AddW3CLogging(options =>
        {
            options.LoggingFields = W3CLoggingFields.All;
            options.AdditionalRequestHeaders.Add("x-forwarded-for");
            options.AdditionalRequestHeaders.Add("x-client-ssl-protocol");
            options.FileSizeLimit = FileSizeLimitInBytes;
            options.RetainedFileCountLimit = 2; // TODO: verify if this needs to change
            options.FileName = fileNamePrefix;
            options.LogDirectory = logDirectory;
            options.FlushInterval = TimeSpan.FromSeconds(FlushIntervalInSeconds);
        });
    }

    public static void UseW3CLogging(this WebApplication app, LoggingConfigurationOptions loggingConfigOptions)
    {
        app.UseW3CLogging();
    }
    
}