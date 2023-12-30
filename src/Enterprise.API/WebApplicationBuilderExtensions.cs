using Enterprise.API.Caching;
using Enterprise.API.Controllers;
using Enterprise.API.Cors;
using Enterprise.API.Cors.Options;
using Enterprise.API.ErrorHandling;
using Enterprise.API.ErrorHandling.Options;
using Enterprise.API.Hosting;
using Enterprise.API.Mapping;
using Enterprise.API.Options;
using Enterprise.API.Security;
using Enterprise.API.Security.Options;
using Enterprise.API.Swagger;
using Enterprise.API.Swagger.Options;
using Enterprise.API.Versioning;
using Enterprise.Logging;
using Enterprise.Logging.Options;
using Enterprise.Monitoring.Health;
using Enterprise.Monitoring.Health.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureServices(this WebApplicationBuilder builder, ApiConfigurationOptions apiConfigOptions)
    {
        ServiceConfigurationOptions options = apiConfigOptions.ServiceConfigurationOptions;

        CorsConfigurationOptions corsConfigOptions = apiConfigOptions.CorsConfigurationOptions;
        ErrorHandlingConfigurationOptions errorHandlingConfigOptions = apiConfigOptions.ErrorHandlingConfigurationOptions;
        HealthCheckOptions healthCheckOptions = apiConfigOptions.HealthCheckOptions;
        LoggingConfigurationOptions loggingConfigOptions = apiConfigOptions.LoggingConfigurationOptions;
        SwaggerConfigurationOptions swaggerConfigOptions = apiConfigOptions.SwaggerConfigurationOptions;

        builder.ConfigureLogging(loggingConfigOptions);
        //builder.ConfigureOpenTelemetry();

        builder.Services.ConfigureCors(builder.Environment, corsConfigOptions);
        builder.Services.ConfigureIISIntegration(options.IISIntegrationOptions);

        builder.Services.ConfigureErrorHandling(builder, errorHandlingConfigOptions);
        builder.Services.ConfigureSecurity(builder, options.JwtBearerTokenOptions);

        builder.Services.ConfigureControllers(options.ControllerConfigurationOptions);

        builder.Services.ConfigureCaching(builder.Configuration, options.CachingConfigurationOptions);
        builder.Services.ConfigureResponseCaching();
        builder.Services.ConfigureOutputCaching();

        // customize problem detail results
        //builder.Services.AddTransient<ProblemDetailsFactory, CustomProblemDetailsFactory>();

        builder.Services.ConfigureSwagger(swaggerConfigOptions);

        // determines the content type of files
        builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

        // required to get HttpContext reference in object instances
        // inject IHttpContextAccessor into constructor of class
        builder.Services.AddHttpContextAccessor();

        // monitoring - health check services
        builder.ConfigureHealthChecks(healthCheckOptions);

        // https://learn.microsoft.com/en-us/aspnet/core/security/anti-request-forgery?view=aspnetcore-8.0#afwma
        // add anti forgery token support for minimal APIs that process form data
        //builder.Services.AddAntiforgery();

        // allow for the rendering of server side razor components
        // makes rendered components available to be returned in API responses
        //builder.Services.AddRazorComponents();

        // registers internal services needed to manage request timeouts
        builder.Services.AddRequestTimeouts();

        // let's us inject clients, and use them more safely without newing them up each time
        builder.Services.AddHttpClient(); // NOTE: you can add named clients, and instance specific configuration in this constructor parameter (duplicate as needed)

        // typed clients - can be configured and injected into specific abstractions / implementations
        // https://app.pluralsight.com/course-player?clipId=7c5b839b-c11d-43ee-94fa-0f07892d53a3
        //builder.Services.AddHttpClient<IUserGateway, UserGateway>();

        builder.Services.ConfigureApiVersioning(options.VersioningConfigurationOptions);

        builder.Services.ConfigureAutoMapper(options.AutoMapperConfigurationOptions);

        // this is a hook for adding custom service registrations
        options.RegisterCustomServices?.Invoke(builder.Services, builder);
    }
}