using Enterprise.API.Hosting.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.Hosting;

/// <summary>
/// ASP.NET Core applications are by default self-hosted.
/// If we want to host our applications on IIS, we need to configure an IIS integration that will eventually help us with the deployment to IIS.
/// </summary>
public static class IISConfigurationService
{
    public static void ConfigureIISIntegration(this IServiceCollection services, IISIntegrationOptions customOptions)
    {
        if (!customOptions.EnableIISIntegration)
            return;

        services.Configure<IISOptions>(options =>
        {
            options.AutomaticAuthentication = customOptions.AutomaticAuthentication;
            options.AuthenticationDisplayName = customOptions.AuthenticationDisplayName;
            options.ForwardClientCertificate = customOptions.ForwardClientCertificate;
        });
    }
}