using Enterprise.API.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.API.Swagger.OperationFilters;

public class RemoveVersionParamsFilter : IOperationFilter
{
    private readonly bool _mediaTypeVersioningEnabled;
    private readonly List<string> _allVersionNames;

    public RemoveVersionParamsFilter(bool mediaTypeVersioningEnabled, List<string> allVersionNames)
    {
        _mediaTypeVersioningEnabled = mediaTypeVersioningEnabled;
        _allVersionNames = allVersionNames;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        bool isUrlVersioned = context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(x =>
        {
            if (x is not RouteAttribute routeAttribute)
                return false;

            string routeTemplate = routeAttribute.Template;
            bool routeTemplateContainsVersion = routeTemplate.Contains(RoutePartials.VersionSegment, StringComparison.Ordinal);
            return routeTemplateContainsVersion;
        });

        List<OpenApiParameter> versionParameters = operation.Parameters
            .Where(p => _allVersionNames.Any(x => p.Name.Equals(x, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        // URI and media type versioning is embedded in the operation and does not require request parameters
        // We want to remove ALL the supported versioning parameters if either URL or media type versioning are used
        if (_mediaTypeVersioningEnabled || isUrlVersioned)
        {
            foreach (OpenApiParameter versionParameter in versionParameters)
                operation.Parameters.Remove(versionParameter);
        }
        else if (versionParameters.Count > 1)
        {
            // If we have multiple versioning parameters, we only need one represented in the swagger UI
            // we'll default to request header, and fall back to query (if supported)

            OpenApiParameter? queryStringParam = versionParameters.FirstOrDefault(x => x.In == ParameterLocation.Query);
            OpenApiParameter? headerParam = versionParameters.FirstOrDefault(x => x.In == ParameterLocation.Header);

            if (queryStringParam != null)
            {
                operation.Parameters.Remove(queryStringParam);
            }
        }
    }
}