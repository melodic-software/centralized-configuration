using Enterprise.API.Security.Options;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using static Enterprise.API.Security.Constants.SecurityConstants;
using static Enterprise.API.Security.Constants.SecurityConstants.Swagger;

namespace Enterprise.API.Security.OAuth.Extensions;

public static class JwtBearerExtensions
{
    public static void AddJwtBearer(this AuthenticationBuilder authBuilder, IConfiguration configuration,
        JwtBearerTokenOptions jwtBearerTokenOptions, IHostEnvironment environment)
    {
        // this can be completely customized
        if (jwtBearerTokenOptions.ConfigureJwtBearerOptions != null)
        {
            authBuilder.AddJwtBearer(authenticationScheme: JwtBearerAuthenticationScheme, jwtBearerTokenOptions.ConfigureJwtBearerOptions);
        }
        else
        {
            string authority = jwtBearerTokenOptions.Authority;
            string audience = jwtBearerTokenOptions.Audience;
            bool requireHttpsMetadata = environment.IsProduction();
            string? validIssuer = GetConfigValue(configuration, JwtValidIssuerConfigKey, jwtBearerTokenOptions.Authority);
            string? validAudience = GetConfigValue(configuration, JwtValidAudienceConfigKey, jwtBearerTokenOptions.Audience);
            string nameClaimType = jwtBearerTokenOptions.NameClaimType ?? JwtClaimTypes.Name;
            string roleClaimType = JwtClaimTypes.Role;

            authBuilder.AddJwtBearer(authenticationScheme: JwtBearerAuthenticationScheme, options =>
            {
                options.Authority = authority;
                options.Audience = audience;
                options.SaveToken = true; // required for "builder.Services.AddOpenIdConnectAccessTokenManagement"
                options.RequireHttpsMetadata = requireHttpsMetadata;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,

                    // audience is not part of the OAuth2 standard, so we're disabling this middleware by default
                    // it is still important to check that the token is intended for the API, but we do that with a separate authorization policy
                    ValidateAudience = false,

                    //ValidateIssuerSigningKey = true,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,

                    // ensures we're only accepting JWT tokens
                    // this prevents so-called "JWT" confusion attacks
                    ValidTypes = new[] { AccessTokenInJwtFormatType },

                    // by default the expiration has a grace period of 5 minutes
                    // so technically expired tokens can be used unless the grace period has elapsed
                    // we can override this if needed...
                    //ClockSkew = TimeSpan.Zero,

                    //IssuerSigningKey = new SymmetricSecurityKey(),

                    NameClaimType = nameClaimType,
                    RoleClaimType = roleClaimType,
                };
            });
        }
    }

    public static void AddJwtBearerSecurityDefinition(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(BearerSecurityDefinitionName, new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Please enter a valid token to access this API",
            Type = SecuritySchemeType.Http, // used for both "bearer" and "basic" authentication
            BearerFormat = "JWT",
            Scheme = BearerSecuritySchemeName
        });
    }

    public static void AddJwtBearerSecurityRequirement(this SwaggerGenOptions options)
    {
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = BearerSecurityDefinitionName
                    }
                },
                new List<string>()
            }
        });
    }

    private static string? GetConfigValue(IConfiguration configuration, string key, string? fallbackValue = null)
    {
        object? configValue = configuration.GetValue(typeof(string), key, null);
        string? configValueString = configValue?.ToString() ?? fallbackValue;
        return configValueString;
    }
}