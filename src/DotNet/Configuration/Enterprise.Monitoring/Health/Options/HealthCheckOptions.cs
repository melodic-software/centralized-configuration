using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Monitoring.Health.Options;

public class HealthCheckOptions
{
    /// <summary>
    /// This is the path / resource that will be exposed
    /// </summary>
    public string UrlPatternName { get; set; } = "health-checks";

    /// <summary>
    /// If enabled, calls to the health check endpoint will not require authentication.
    /// </summary>
    public bool AllowAnonymous { get; set; } = true;

    /// <summary>
    /// If provided, a health check will be enabled for the OpenIdConnect provider.
    /// This is typically a security token service like "IdentityServer".
    /// </summary>
    public string? OpenIdConnectAuthorityUri { get; set; } = null;

    /// <summary>
    /// Allows for custom registrations of health checks specific to the executing application.
    /// These can be entity framework DB contexts, SQL server databases, etc.
    /// </summary>
    public Action<IHealthChecksBuilder> AddHealthChecks { get; set; }

    public HealthCheckOptions()
    {
        AddHealthChecks = _ => { };
    }
}