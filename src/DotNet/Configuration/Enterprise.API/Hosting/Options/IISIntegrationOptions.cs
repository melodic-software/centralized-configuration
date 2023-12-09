namespace Enterprise.API.Hosting.Options;

public class IISIntegrationOptions
{
    public bool EnableIISIntegration { get; set; }

    /// <summary>
    /// When set to true, the authentication middleware sets the HttpContext.User and responds to generic challenges.
    /// If false, the authentication middleware only provides an identity (HttpContext.User) and responds to challenges
    /// when explicitly requested by the authentication scheme.
    /// NOTE: Windows authentication must be enabled in IIS for automatic authentication to function.
    /// </summary>
    public bool AutomaticAuthentication { get; set; }

    /// <summary>
    /// Sets the display name shown to users on login pages.
    /// </summary>
    public string? AuthenticationDisplayName { get; set; }

    /// <summary>
    /// The HttpContext.Connection.ClientCertificate is populated when this is set to true,
    /// AND the "MS-ASPNETCORE-CLIENTCERT" request header is present.
    /// </summary>
    public bool ForwardClientCertificate { get; set; }

    public IISIntegrationOptions()
    {
        EnableIISIntegration = true;
        AutomaticAuthentication = true;
        AuthenticationDisplayName = null;
        ForwardClientCertificate = true;
    }
}