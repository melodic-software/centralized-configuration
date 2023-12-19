using Enterprise.Monitoring.Health.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Enterprise.Monitoring.Health;

public static class HealthCheckConfigurationService
{
    // TODO: explore more here: https://app.pluralsight.com/library/courses/asp-dot-net-core-health-checks/table-of-contents

    public static void ConfigureHealthChecks(this WebApplicationBuilder builder, HealthCheckOptions options)
    {
        IHealthChecksBuilder healthCheckBuilder = builder.Services.AddHealthChecks();
            
        if (!string.IsNullOrWhiteSpace(options.OpenIdConnectAuthorityUri))
        {
            // TODO: do we want the status to be unhealthy?
            // if we have token validation middleware enabled, won't this fail?
            healthCheckBuilder.AddIdentityServer(new Uri(options.OpenIdConnectAuthorityUri), failureStatus: HealthStatus.Degraded);
        }

        // allow for application specific health checks like entity framework DB contexts, etc.
        // these are the additional health checks services that can be registered:
        // https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks
        options.AddHealthChecks.Invoke(healthCheckBuilder);
    }

    public static void UseHealthChecks(this WebApplication app, HealthCheckOptions options)
    {
        IEndpointConventionBuilder builder = app.MapHealthChecks(options.UrlPatternName);

        if (options.AllowAnonymous)
        {
            // any calls to the health check endpoint will not require authentication
            builder.AllowAnonymous();
        }
    }
}