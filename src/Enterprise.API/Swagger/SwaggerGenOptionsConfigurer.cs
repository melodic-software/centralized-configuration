using Enterprise.API.Swagger.DocumentFilters;
using Enterprise.API.Swagger.Extensions;
using Enterprise.API.Swagger.OperationFilters;
using Enterprise.API.Swagger.Options;
using Enterprise.API.Versioning.Constants;
using Enterprise.API.Versioning.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Enterprise.API.Swagger.Documents.DocumentInclusionService;
using static Enterprise.API.Swagger.Documents.SwaggerDocumentService;

namespace Enterprise.API.Swagger;

public class SwaggerGenOptionsConfigurer : IConfigureOptions<SwaggerGenOptions>
{
    private readonly SwaggerConfigurationOptions _swaggerConfigOptions;
    private readonly VersioningConfigurationOptions _versioningConfigOptions;
    private readonly IConfiguration _config;
    private readonly ILogger<SwaggerGenOptionsConfigurer> _logger;
    private readonly IServiceProvider _serviceProvider;

    public SwaggerGenOptionsConfigurer(SwaggerConfigurationOptions swaggerConfigOptions,
        VersioningConfigurationOptions versioningConfigOptions,
        IConfiguration config,
        ILogger<SwaggerGenOptionsConfigurer> logger,
        IServiceProvider serviceProvider)
    {
        _swaggerConfigOptions = swaggerConfigOptions;
        _versioningConfigOptions = versioningConfigOptions;
        _config = config;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        try
        {
            if (_swaggerConfigOptions.CustomConfigure != null)
            {
                // this is a full customization of the swagger spec generation
                _swaggerConfigOptions.CustomConfigure(options);
                return;
            }

            // the document name is used in the path:
            // swagger/{documentName}/swagger.json
            // ex: https://localhost:5000/swagger/v1/swagger.json

            // separate documents can be created centered around specific resources
            // and controllers can be decorated with the [ApiExplorerSettings] attribute
            // specifying a group name that matches the swagger document name
            // https://app.pluralsight.com/course-player?clipId=e04fd8bb-f0a8-4048-a55c-dd9eae937613

            // the other more popular approach is to create documents for specific versions and to let the resources all co-exist per version
            // this is what we do by default...

            ConfigureSwaggerDocuments(options, _swaggerConfigOptions, _serviceProvider, _config);

            options.DocInclusionPredicate(CanIncludeDocument);
                
            //options.ResolveConflictingActions(ConflictingActionResolver.ResolveSimple);

            options.IgnoreObsoleteActions();
            options.IgnoreObsoleteProperties();

            // every class in the swagger JSON must have a unique schemaId
            // Swashbuckle tries to just use the class name as a simple schemaId,
            // however if you have two classes in different namespaces with the same name this will not work
            // this outputs full assembly qualified names for models under the "schemas" area at the bottom of the Swagger UI
            options.CustomSchemaIds(type => type.FullName);
                
            options.AddSecurity(_swaggerConfigOptions);
            options.AddXmlComments(_swaggerConfigOptions);

            AddDocumentFilters(options);
            AddOperationFilters(options, _versioningConfigOptions);

            OrderActions(options);

            // allow for adding application specific configuration
            _swaggerConfigOptions.PostConfigure?.Invoke(options);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error loading discovery document for Swagger UI.");
        }
    }

    private static void AddDocumentFilters(SwaggerGenOptions options)
    {
        options.DocumentFilter<PathLowercaseDocumentFilter>();
        options.DocumentFilter<AlphabeticSortingFilter>();
        options.DocumentFilter<CustomDocumentFilter>();
    }

    private static void AddOperationFilters(SwaggerGenOptions options, VersioningConfigurationOptions versionConfigOptions)
    {
        bool mediaTypeVersioningEnabled = versionConfigOptions.EnableMediaTypeVersioning;

        // TODO: Inject this in as configuration, particularly if this can be customized by each API instance.
        // For now, we're just going to rely on the preconfigured constant values.
        List<string> allVersionNames =
        [
            VersioningConstants.VersionQueryStringParameterName,
            VersioningConstants.CustomVersionRequestHeader,
            // Add other version parameter names as needed
        ];

        options.OperationFilter<NonApplicableParamFilter>();
        options.OperationFilter<RemoveVersionParamsFilter>(mediaTypeVersioningEnabled, allVersionNames);
        options.OperationFilter<SetDefaultVersionParamValueFilter>(allVersionNames);
    }

    private static void OrderActions(SwaggerGenOptions options)
    {
        // https://terencegolla.com/.net/swashbucklecustom-ordering-of-controllers/
        string[] methodsOrder = { "get", "put", "patch", "post", "delete", "head", "options", "trace" };

        //options.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}");

        options.OrderActionsBy(apiDesc =>
        {
            string? controller = apiDesc.ActionDescriptor.RouteValues["controller"];
            string? httpMethod = apiDesc.HttpMethod?.ToLower();
            int indexOf = Array.IndexOf(methodsOrder, httpMethod);
            string sortKey = $"{controller}_{indexOf}";
            return sortKey;
        });
    }
}