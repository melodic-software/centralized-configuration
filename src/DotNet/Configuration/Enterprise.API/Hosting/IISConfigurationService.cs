using Enterprise.API.Hosting.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.Hosting;

public static class IISConfigurationService
{
    public static void ConfigureIISIntegration(this IServiceCollection services, IISIntegrationOptions iisOptions)
    {
        if (!iisOptions.EnableIISIntegration)
            return;

        services.Configure<IISOptions>(options =>
        {
            options.AutomaticAuthentication = iisOptions.AutomaticAuthentication;
            options.AuthenticationDisplayName = iisOptions.AuthenticationDisplayName;
            options.ForwardClientCertificate = iisOptions.ForwardClientCertificate;
        });
    }
}