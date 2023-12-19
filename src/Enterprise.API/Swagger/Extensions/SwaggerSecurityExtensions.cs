using Enterprise.API.Security.OAuth.Extensions;
using Enterprise.API.Swagger.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.API.Swagger.Extensions;

public static class SwaggerSecurityExtensions
{
    public static void AddSecurity(this SwaggerGenOptions options, SwaggerConfigurationOptions swaggerConfigOptions)
    {
        options.AddSecurityDefinitions(swaggerConfigOptions);
        options.AddSecurityRequirements(swaggerConfigOptions);
    }

    private static void AddSecurityDefinitions(this SwaggerGenOptions options, SwaggerConfigurationOptions swaggerConfigOptions)
    {
        //options.AddApiKeySecurityDefinition();
        //options.AddBasicAuthenticationSecurityDefinition(swaggerConfigOptions);
        //options.AddBearerSecurityDefinition();
        options.AddOAuth2SecurityDefinition(swaggerConfigOptions);
    }

    private static void AddSecurityRequirements(this SwaggerGenOptions options, SwaggerConfigurationOptions swaggerConfigOptions)
    {
        //options.AddApiKeySecurityRequirement();
        //options.AddBasicAuthenticationSecurityRequirement(swaggerConfigOptions);
        //options.AddBearerSecurityRequirement();
        options.AddOAuth2SecurityRequirement(swaggerConfigOptions);
    }
}