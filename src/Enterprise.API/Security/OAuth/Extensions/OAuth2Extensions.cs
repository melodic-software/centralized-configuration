using Enterprise.API.Swagger.Options;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Enterprise.API.Security.Constants.SecurityConstants;
using static Enterprise.API.Security.Constants.SecurityConstants.Swagger;

namespace Enterprise.API.Security.OAuth.Extensions;

public static class OAuth2Extensions
{
    /// <summary>
    /// NOTE: use this OR "AddJwtBearer" - not both
    /// https://github.com/IdentityModel/IdentityModel.AspNetCore.OAuth2Introspection
    /// This involves reference tokens, and increases load on the identity server AND the API
    /// because the API has to make a call for every request
    /// </summary>
    /// <param name="authBuilder"></param>
    /// <param name="authority"></param>
    /// <param name="clientId"></param>
    /// <param name="clientSecret"></param>
    /// <param name="cacheDurationInMinutes"></param>
    public static void AddOAuth2Introspection(this AuthenticationBuilder authBuilder, string authority,
        string? clientId = null, string? clientSecret = null, int cacheDurationInMinutes = 5)
    {
        authBuilder.AddOAuth2Introspection(authenticationScheme: JwtBearerAuthenticationScheme, options =>
        {
            options.Authority = authority;
            options.ClientId = clientId ?? "configuration-api-REPLACE-ME";
            options.ClientSecret = clientSecret ?? "259439594-238128-REPLACE-ME";
            // there is built in caching, with a default of 5 minutes, but we can override that
            options.CacheDuration = TimeSpan.FromMinutes(cacheDurationInMinutes);
        });
    }

    public static void AddOAuth2SecurityDefinition(this SwaggerGenOptions options, SwaggerConfigurationOptions swaggerConfigOptions)
    {
        if (string.IsNullOrWhiteSpace(swaggerConfigOptions.Authority))
        {
            Console.WriteLine("The trusted authority has not been configured. The Swagger OAuth2 security definition will not be defined.");
            return;
        }

        string authority = swaggerConfigOptions.Authority;
        Dictionary<string, string> oAuthScopes = swaggerConfigOptions.OAuthScopes;

        DiscoveryDocumentResponse discoveryDocResponse;

        try
        {
            discoveryDocResponse = DiscoveryDocumentService
                .GetDiscoveryDocumentAsync(authority)
                .GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }

        DiscoveryDocumentService.ValidateDiscoveryDocumentResponse(discoveryDocResponse);

        string authorizeEndpoint = discoveryDocResponse.AuthorizeEndpoint ?? string.Empty;
        string tokenEndpoint = discoveryDocResponse.TokenEndpoint ?? string.Empty;

        options.AddSecurityDefinition(OAuth2SecurityDefinitionName, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(authorizeEndpoint),
                    TokenUrl = new Uri(tokenEndpoint),
                    Scopes = oAuthScopes
                }
            }
        });
    }

    public static void AddOAuth2SecurityRequirement(this SwaggerGenOptions options, SwaggerConfigurationOptions swaggerConfigOptions)
    {
        Dictionary<string, string> oAuthScopes = swaggerConfigOptions.OAuthScopes;
        string[] scopeNames = oAuthScopes.Keys.ToArray();

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = OAuth2SecurityDefinitionName
                    }
                },
                scopeNames
            }
        });
    }
}