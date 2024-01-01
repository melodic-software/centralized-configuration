using Enterprise.API.Constants;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.API.Swagger.OperationFilters;

public class NonApplicableParamFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        List<OpenApiParameter> parametersToRemove = operation.Parameters
            .Where(p =>
            {
                bool isAcceptHeader = p.In == ParameterLocation.Header &&
                                      p.Name.Equals(HttpHeaderConstants.Accept, StringComparison.OrdinalIgnoreCase);

                return isAcceptHeader;
            })
            .ToList();

        foreach (OpenApiParameter parameter in parametersToRemove)
            operation.Parameters.Remove(parameter);
    }
}