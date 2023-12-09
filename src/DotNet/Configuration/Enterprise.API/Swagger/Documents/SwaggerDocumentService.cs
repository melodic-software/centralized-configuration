using Asp.Versioning.ApiExplorer;
using Enterprise.API.Swagger.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.API.Swagger.Documents;

public static class SwaggerDocumentService
{
    public static void ConfigureSwaggerDocuments(SwaggerGenOptions options, SwaggerConfigurationOptions swaggerConfigOptions, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        IApiVersionDescriptionProvider? apiVersionDescriptionProvider = serviceProvider.GetService<IApiVersionDescriptionProvider>();

        if (apiVersionDescriptionProvider == null)
            throw new Exception($"{nameof(apiVersionDescriptionProvider)} cannot be null");

        string? environmentName = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");

        // register separate swagger documents per version
        foreach (ApiVersionDescription description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            string swaggerDocumentName = description.GroupName;
            string version = description.ApiVersion.ToString();
            var title = swaggerConfigOptions.ApplicationName;

            if (!string.IsNullOrWhiteSpace(environmentName))
                title += $" ({environmentName})";

            OpenApiInfo openApiInfo = new OpenApiInfo
            {
                Version = version,
                Title = title,
                Description = swaggerConfigOptions.ApplicationDescription,
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Example Contact",
                    Email = "john.doe@example.com",
                    Url = new Uri("https://example.com/contact")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            };

            if (description.IsDeprecated)
            {
                // TODO: make this configurable?
                // at least check the current description ending and dynamically add a sentence or parenthesized value
                openApiInfo.Description += " This API version has been deprecated.";
            }

            options.SwaggerDoc(swaggerDocumentName, openApiInfo);
        }
    }
}