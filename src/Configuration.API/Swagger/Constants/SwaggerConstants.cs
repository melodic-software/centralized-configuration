using Configuration.API.Constants;

namespace Configuration.API.Swagger.Constants;

public static class SwaggerConstants
{
    public const string OAuthAppName = ApplicationConstants.ApplicationDisplayName;

    public static readonly Dictionary<string, string> OAuthScopes = new()
    {
        { "configuration-api", "Access to the Configuration API" },
        { "openid", "OpenID information" },
        { "profile", "User profile information" },
        { "email", "User email address" }
    };
}