using Enterprise.API.Security.OAuth.Extensions;
using Enterprise.API.Security.Options;
using Enterprise.Logging.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;
using Enterprise.Hosting.Extensions;
using static Enterprise.API.Security.Constants.SecurityConstants;

namespace Enterprise.API.Security;

public static class SecurityConfigurationService
{
    // TODO: Add option to enable/disable security.
    // Only allow disabling if running locally. Throw an error if running in any other environment.

    public static void ConfigureSecurity(this IServiceCollection services, WebApplicationBuilder builder, JwtBearerTokenOptions jwtBearerTokenOptions)
    {
        JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        AddAuthentication(builder, services, jwtBearerTokenOptions);
        AddAuthorization(builder, services);
    }

    public static void UseSecurity(this WebApplication appBuilder, IWebHostEnvironment env)
    {
        appBuilder.UseAuthentication();
        appBuilder.UseMiddleware<UserScopeMiddleware>(); // this will add user information (via a logging scope) if a user is authenticated
        appBuilder.UseAuthorization();

        if (env.IsLocal() || env.IsDevelopment())
        {
            // some errors may be obfuscated by this
            // do NOT enable this in production
            IdentityModelEventSource.ShowPII = true;
        }
    }

    private static void AddAuthentication(WebApplicationBuilder builder, IServiceCollection services, JwtBearerTokenOptions jwtBearerTokenOptions)
    {
        ConfigurationManager configuration = builder.Configuration;

        AuthenticationBuilder authBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = DefaultAuthenticationScheme;
            options.DefaultChallengeScheme = DefaultAuthenticationScheme;
            options.DefaultScheme = DefaultAuthenticationScheme;
        });

        //authBuilder.AddBasicAuthentication();
        authBuilder.AddJwtBearer(configuration, jwtBearerTokenOptions, builder.Environment);
        //authBuilder.AddOAuth2Introspection(authority: jwtBearerTokenOptions.Authority);
    }

    private static void AddAuthorization(WebApplicationBuilder builder, IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // these policy names can be assigned to the Policy property on Authorize attributes on a controller class or method
            // requests that do not fulfill the policy will return a 403 "forbidden" status code
            options.AddPolicy(DefaultAuthPolicyName, policy =>
            {
                policy.RequireAuthenticatedUser();
                // add more policy rules here
            });

            // TODO: add delegate to add custom authorization policies
            // https://app.pluralsight.com/course-player?clipId=6f802090-fb1c-4618-b1ae-fbb54c13d789

            //options.AddDefaultPolicy();
            //options.AddDefaultFallbackPolicy();

            // https://app.pluralsight.com/course-player?clipId=7a3e77a2-2a7c-4177-825b-a51009cb36b7

            // authorization API
            // https://app.pluralsight.com/course-player?clipId=c734bcbd-061e-493e-81bb-fe1e258ee652
            // if a separate authorization API is used and you don't want to reuse access tokens between APIs (poor mans delegation)
            // take a look at the token exchange (a custom grant type extension implemented in IdentityServer
            // https://docs.duendesoftware.com/identityserver/v5/tokens/extension_grants/token_exchange/

            // requirements and handlers
            // https://app.pluralsight.com/course-player?clipId=48dfa72f-ab0c-4c06-9b84-6d92a56ee26f
        });
    }
}