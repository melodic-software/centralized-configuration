using Enterprise.API.Security.ApiKey.Middleware;
using Enterprise.API.Security.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Enterprise.API.Security.Constants.SecurityConstants.Swagger;

namespace Enterprise.API.Security.ApiKey.Extensions;

public static class ApiKeyExtensions
{
    public static void UseApiKeyMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ApiKeyMiddleware>();
    }

    public static void AddApiKeySecurityDefinition(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(ApiKeySecurityDefinitionName, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = SecurityConstants.CustomApiKeyHeader,
            Description = "Please ensure an API key is present in the request headers",
            Type = SecuritySchemeType.ApiKey,
            Scheme = ApiKeySecuritySchemeName
        });
    }

    public static void AddApiKeySecurityRequirement(this SwaggerGenOptions options)
    {
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = ApiKeySecurityDefinitionName
                    }
                },
                new List<string>()
            }
        });
    }
}