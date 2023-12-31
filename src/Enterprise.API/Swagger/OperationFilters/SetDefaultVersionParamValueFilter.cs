using System.Text.RegularExpressions;
using Asp.Versioning;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.API.Swagger.OperationFilters
{
    public class SetDefaultVersionParamValueFilter(List<string> allVersionNames) : IOperationFilter
    {
        private const string DocumentVersionRegexPattern = @"^v\d+$";

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            ApiVersion? apiVersion = context.ApiDescription.ActionDescriptor.EndpointMetadata
                .OfType<ApiVersionAttribute>()
                .SelectMany(attr => attr.Versions)
                .FirstOrDefault();

            if (apiVersion == null)
                return;
            
            List<OpenApiParameter> versionParameters = operation.Parameters
                .Where(p => allVersionNames.Any(x => p.Name.Equals(x, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            foreach (OpenApiParameter versionParameter in versionParameters)
            {
                //versionParameter.Description = "API Version";
                versionParameter.Example = new OpenApiString(apiVersion.ToString());
                versionParameter.AllowEmptyValue = false;

                // These operations should be grouped in specific versioned documents (by default)
                // It wouldn't make sense for users to be able to change the version value
                // We add a check here in case the default conventions are not followed
                if (Regex.IsMatch(context.DocumentName, DocumentVersionRegexPattern))
                    versionParameter.Schema.ReadOnly = true;
            }
        }
    }
}
