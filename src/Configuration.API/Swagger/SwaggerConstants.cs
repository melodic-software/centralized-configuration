using Configuration.API.Constants;

namespace Configuration.API.Swagger;

public static class SwaggerConstants
{
    public const string OAuthAppName = ApplicationConstants.ApplicationDisplayName;

    public static readonly Dictionary<string, string> OAuthScopes = new()
    {
        { "api", "Access to the API" },
        { "openid", "OpenID information" },
        { "profile", "User profile information" },
        { "email", "User email address" }
    };
}