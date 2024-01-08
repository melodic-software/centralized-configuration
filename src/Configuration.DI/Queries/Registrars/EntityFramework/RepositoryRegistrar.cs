using Configuration.ApplicationServices.Applications.Shared;
using Configuration.EntityFramework.DbContexts.Configuration;
using Configuration.EntityFramework.Queries.Repositories;
using Enterprise.Mapping.Properties.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Configuration.DI.Queries.Registrars.EntityFramework;

internal static class RepositoryRegistrar
{
    internal static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
        {
            ConfigurationContext configurationContext = provider.GetRequiredService<ConfigurationContext>();
            IPropertyMappingService propertyMappingService = provider.GetRequiredService<IPropertyMappingService>();
            ILogger<ApplicationRepository> logger = provider.GetRequiredService<ILogger<ApplicationRepository>>();
            IApplicationRepository applicationRepository = new ApplicationRepository(configurationContext, propertyMappingService, logger);
            return applicationRepository;
        });
    }
}