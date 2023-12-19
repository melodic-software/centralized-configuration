using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.API.Swagger.DocumentFilters;

public class CustomDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        // TODO: apply global document customizations (non application specific)

        OpenApiInfo? info = swaggerDoc.Info;
        string? version = info?.Version;
    }
}