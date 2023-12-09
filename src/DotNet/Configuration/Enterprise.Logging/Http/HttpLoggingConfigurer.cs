using Enterprise.Logging.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Logging.Http;

public static class HttpLoggingConfigurer
{
    public static void ConfigureHttpLogging(this IServiceCollection services, LoggingConfigurationOptions loggingConfigOptions)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging

        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.All;

            // request headers that are allowed to be logged
            options.RequestHeaders.Add("sec-ch-ua");

            options.MediaTypeOptions.AddText("application/javascript");
            options.RequestBodyLogLimit = 4096;
            options.ResponseBodyLogLimit = 4096;
        });
    }

    public static void UseHttpLogging(this WebApplication app, LoggingConfigurationOptions loggingConfigOptions)
    {
        app.UseHttpLogging();
    }
}