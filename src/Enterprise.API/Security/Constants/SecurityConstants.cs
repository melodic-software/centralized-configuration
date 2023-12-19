using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Enterprise.API.Security.Constants;

public static class SecurityConstants
{
    // https://datatracker.ietf.org/doc/html/rfc9068
    public const string AccessTokenInJwtFormatType = "at+jwt"; 
    public const string BasicAuthenticationScheme = "Basic";
    public const string CustomApiKeyHeader = "X-API-Key";
    public const string DefaultAuthenticationScheme = JwtBearerAuthenticationScheme;
    public const string DefaultAuthPolicyName = "DefaultAuthPolicy";
    public const string DefaultJwtNameClaimType = "email"; // TODO: shouldn't this be JwtClaimTypes.Name?
    public const string JwtBearerAuthenticationScheme = JwtBearerDefaults.AuthenticationScheme; // "Bearer"
    public const string JwtValidAudienceConfigKey = "JWT:ValidAudience";
    public const string JwtValidIssuerConfigKey = "JWT:ValidIssuer";

    public static class Swagger
    {
        public const string ApiKeySecurityDefinitionName = "ApiKey";
        public const string ApiKeySecuritySchemeName = "ApiKeyScheme";
        public const string BasicAuthenticationSecurityDefinitionName = "basicAuth";
        public const string BasicAuthenticationSecuritySchemeName = "basic";
        public const string BearerSecurityDefinitionName = "Bearer";
        public const string BearerSecuritySchemeName = "Bearer";
        public const string OAuth2SecurityDefinitionName = "oauth2";
    }
}