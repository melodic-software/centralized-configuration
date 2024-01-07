using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.Routing;
using Enterprise.API.ContentNegotiation.Constants;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Configuration.API.Swagger.OperationFilters;

public class GetApplicationsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (string.IsNullOrWhiteSpace(operation.OperationId))
            return;

        if (operation.OperationId != RouteNames.GetApplications)
            return;

        // TODO: explore using extensions (containing the schema type, and a flag to add this for the given operation
        // there's got to be a way to make this more reusable for resolution / combining actions that support hypermedia and non hypermedia models
        // NOTE: the conflicting action resolver can't be used for this since the dictionary is based on the status code

        // operation.Extensions

        // TODO: can we make the schema a hypermedia model?
        OpenApiSchema? schema = context.SchemaGenerator.GenerateSchema(typeof(List<ApplicationDto>), context.SchemaRepository);
        OpenApiMediaType openApiMediaType = new OpenApiMediaType { Schema = schema };

        string statusCode = StatusCodes.Status200OK.ToString();

        OpenApiResponse? openApiResponse = operation.Responses[statusCode];

        openApiResponse.Content.Add(VendorMediaTypeConstants.HypermediaJson, openApiMediaType);
    }
}