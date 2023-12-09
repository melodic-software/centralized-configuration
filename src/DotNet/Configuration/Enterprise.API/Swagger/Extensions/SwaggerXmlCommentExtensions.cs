using System.Reflection;
using Enterprise.API.Swagger.Options;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.API.Swagger.Extensions;

public static class SwaggerXmlCommentExtensions
{
    public static void AddXmlComments(this SwaggerGenOptions options, SwaggerConfigurationOptions swaggerConfigOptions)
    {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();

        // this should be the main application
        Assembly? entryAssembly = Assembly.GetEntryAssembly();

        // if an API client library is defined and the delegate reference has been provided
        // we can include the comments associated with those API contract models (if applicable)
        Assembly? apiClientAssembly = swaggerConfigOptions.GetApiClientAssembly?.Invoke();

        AddXmlComments(entryAssembly, options);
        AddXmlComments(apiClientAssembly, options);
    }

    private static void AddXmlComments(Assembly? assembly, SwaggerGenOptions options)
    {
        if (assembly == null)
            return;

        string baseDirectory = AppContext.BaseDirectory;

        string? assemblyName = assembly.GetName().Name;
        string xmlCommentsFilename = $"{assemblyName}.xml";
        string xmlCommentsFilePath = Path.Combine(baseDirectory, xmlCommentsFilename);

        if (File.Exists(xmlCommentsFilePath))
        {
            options.IncludeXmlComments(xmlCommentsFilePath);
        }
        else
        {
            // throw configuration exception?
        }
    }
}