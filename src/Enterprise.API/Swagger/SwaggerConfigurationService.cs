using Enterprise.API.Swagger.Constants;
using Enterprise.API.Swagger.Options;
using Enterprise.API.Versioning.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Enterprise.API.Swagger.SwaggerUIConfigurationService;

namespace Enterprise.API.Swagger;
// https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle

public static class SwaggerConfigurationService
{
    public static void ConfigureSwagger(this IServiceCollection services, SwaggerConfigurationOptions swaggerConfigOptions, VersioningConfigurationOptions versionConfigOptions)
    {
        if (!swaggerConfigOptions.EnableSwagger)
            return;

        // exposes information on the API, which is used internally by Swashbuckle to create an OpenApi specification
        services.AddEndpointsApiExplorer();

        services.AddTransient(serviceProvider =>
        {
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            ILogger<SwaggerGenOptionsConfigurer> logger = serviceProvider.GetRequiredService<ILogger<SwaggerGenOptionsConfigurer>>();

            // this is our primary configurer for swagger generation instead of the setupAction that can be passed into services.AddSwaggerGen()
            // we can inject other services in here as needed, which is one advantage over calling .AddSwaggerGen on the IServiceCollection instance
            IConfigureOptions<SwaggerGenOptions> result = new SwaggerGenOptionsConfigurer(swaggerConfigOptions,
                versionConfigOptions, configuration, logger, serviceProvider);

            return result;
        });

        // NOTE: the setup action for swagger generation is not required here
        // it is handled by the IConfigureOptions<SwaggerGenOptions> registered above
        services.AddSwaggerGen();
    }

    public static void UseSwagger(this WebApplication app, SwaggerConfigurationOptions swaggerConfigOptions)
    {
        if (!swaggerConfigOptions.EnableSwagger)
            return;

        // add the middleware that generates the OpenAPI specification
        app.UseSwagger(options =>
        {
            options.RouteTemplate = SwaggerConstants.RoutePrefix + "/{documentname}/swagger.json";
        });

        // adds the middleware that uses the spec to generate the Swagger UI
        app.UseSwaggerUI(options => ConfigureSwaggerUI(options, swaggerConfigOptions, app.Services, app.Configuration));

        // this was causing invalid resource URIs to return a 200 OK instead of a 404
        //app.MapFallback(() => Results.Redirect("/swagger"));
    }
}