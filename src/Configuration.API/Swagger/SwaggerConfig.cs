using Configuration.API.Swagger.OperationFilters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Configuration.API.Swagger;

public static class SwaggerConfig
{
    public static Action<SwaggerGenOptions> PostConfigure = options =>
    {
        // NOTE: operation filters will need to be added for input model variants as well
        // for example: extended/versioned models for the same resource (POST)
        // you will need to add [ApiExplorerSettings(IgnoreApi = true)] to the extended/versioned action methods

        // output model variants
        // standard application/json, application/xml versus application/vnd.company.hateoas+json
        options.OperationFilter<GetApplicationsOperationFilter>();
    };
}