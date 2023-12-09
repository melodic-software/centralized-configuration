using Enterprise.API.Swagger.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Enterprise.API.Security.Constants.SecurityConstants;
using static Enterprise.API.Security.Constants.SecurityConstants.Swagger;

namespace Enterprise.API.Security.Basic.Extensions;

public static class BasicAuthenticationExtensions
{
    public static void AddBasicAuthentication(this AuthenticationBuilder authBuilder)
    {
        authBuilder.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationScheme, null);
    }

    public static void AddBasicAuthenticationSecurityDefinition(this SwaggerGenOptions options, SwaggerConfigurationOptions swaggerConfigOptions)
    {
        options.AddSecurityDefinition(BasicAuthenticationSecurityDefinitionName, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = BasicAuthenticationSecuritySchemeName,
            Description = "Input your username and password to access this API"
        });
    }

    public static void AddBasicAuthenticationSecurityRequirement(this SwaggerGenOptions options, SwaggerConfigurationOptions swaggerConfigOptions)
    {
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = BasicAuthenticationSecurityDefinitionName
                    }
                }, new List<string>() // this is used when working with tokens and scopes, and is not applicable here
            }
        });
    }
}