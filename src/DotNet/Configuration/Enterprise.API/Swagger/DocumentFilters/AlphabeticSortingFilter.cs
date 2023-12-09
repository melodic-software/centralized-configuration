using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.API.Swagger.DocumentFilters;

public class AlphabeticSortingFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        List<KeyValuePair<string, OpenApiPathItem>> paths = swaggerDoc.Paths
            .OrderBy(e => e.Key)
            .ToList();

        OpenApiPaths openApiPaths = new OpenApiPaths();
        paths.ForEach(x => openApiPaths.Add(x.Key, x.Value));

        swaggerDoc.Paths = openApiPaths;
    }
}