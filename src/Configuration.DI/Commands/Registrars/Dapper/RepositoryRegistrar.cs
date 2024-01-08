using Configuration.Dapper.Commands.Repositories;
using Configuration.Domain.Applications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Configuration.DI.Commands.Registrars.Dapper;

internal static class RepositoryRegistrar
{
    internal static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            ILoggerFactory loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            IApplicationRepository applicationRepository = new ApplicationRepository(loggerFactory);
            return applicationRepository;
        });
    }
}