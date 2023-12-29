using Asp.Versioning.ApiExplorer;
using Enterprise.API.Swagger.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using static Enterprise.API.Swagger.Constants.SwaggerConstants;
using static Enterprise.Constants.CharacterConstants;

namespace Enterprise.API.Swagger;

public static class SwaggerUIConfigurationService
{
    public static void ConfigureSwaggerUI(SwaggerUIOptions options, SwaggerConfigurationOptions swaggerConfigOptions, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        if (swaggerConfigOptions.CanConfigureOAuth)
        {
            options.OAuthClientId(swaggerConfigOptions.OAuthClientId);
            options.OAuthAppName(swaggerConfigOptions.OAuthAppName);

            if (swaggerConfigOptions.UsePkce)
                options.OAuthUsePkce();
        }

        IApiVersionDescriptionProvider? apiVersionDescriptionProvider = serviceProvider.GetService<IApiVersionDescriptionProvider>();

        if (apiVersionDescriptionProvider == null)
            throw new Exception($"{nameof(apiVersionDescriptionProvider)} cannot be null");

        foreach (ApiVersionDescription apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            string url = GetEndpointUrl(apiVersionDescription);
            string name = apiVersionDescription.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }

        options.RoutePrefix = RoutePrefix;

        CustomizeUI(options, configuration);
    }

    private static string GetEndpointUrl(ApiVersionDescription apiVersionDescription)
    {
        string url = $"{ForwardSlash}";

        if (!string.IsNullOrWhiteSpace(RoutePrefix))
            url += $"{RoutePrefix}{ForwardSlash}";

        url += $"{apiVersionDescription.GroupName}/swagger.json";

        return url;
    }

    private static void CustomizeUI(SwaggerUIOptions options, IConfiguration configuration)
    {
        options.DefaultModelExpandDepth(2);
        options.DefaultModelRendering(ModelRendering.Example);
        options.DocExpansion(DocExpansion.None);
        options.EnableDeepLinking();
        options.DisplayOperationId();

        options.DisplayRequestDuration();
        options.ShowExtensions();
        options.EnableFilter();

        //options.InjectStylesheet("/swagger-ui/custom.css");
        //options.InjectJavascript("https://code.jquery.com/jquery-3.6.0.min.js");
        //options.InjectJavascript("/swagger-ui/custom.js");

        // allows for complete customization with an embedded index.html resource
        // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/src/Swashbuckle.AspNetCore.SwaggerUI/index.html

        //options.IndexStream = () =>
        //{
        //    Type type = typeof(SwaggerConfigurationOptions);
        //    Assembly assembly = type.Assembly;
        //    string? assemblyName = assembly.GetName().Name;
        //    string relativeNamespace = "Swagger.EmbeddedAssets";
        //    string fileName = "index.html";
        //    string embeddedAssetName = $"{assemblyName}.{relativeNamespace}.{fileName}";
        //    Stream? indexStream = assembly.GetManifestResourceStream(embeddedAssetName);
        //    return indexStream;
        //};
    }
}