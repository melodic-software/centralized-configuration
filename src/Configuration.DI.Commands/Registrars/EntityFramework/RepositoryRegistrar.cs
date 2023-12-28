using Configuration.Core.Domain.Services.Abstract.Repositories;
using Configuration.EntityFramework.Commands.Repositories;
using Configuration.EntityFramework.DbContexts.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Configuration.DI.Commands.Registrars.EntityFramework;

internal static class RepositoryRegistrar
{
    internal static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            ConfigurationContext configurationContext = provider.GetRequiredService<ConfigurationContext>();
            ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            IApplicationRepository applicationRepository = new ApplicationRepository(configurationContext, loggerFactory);
            return applicationRepository;
        });
    }
}