namespace Configuration.API.Security.Constants;

public static class SecurityConstants
{
    public const string Authority = "https://demo.duendesoftware.com"; // TODO: this needs to be updated
    public const string Audience = "api";
    public const string NameClaimType = "email"; // TODO: shouldn't this be JwtClaimTypes.Name?

    public static readonly Dictionary<string, string> OAuthScopes = new()
    {
        { "configuration-api", "Access to the Configuration API" },
        { "openid", "OpenID information" },
        { "profile", "User profile information" },
        { "email", "User email address" }
    };
}