using Configuration.ApplicationServices.Applications.Shared;
using Configuration.Dapper.Queries.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Configuration.DI.Queries.Registrars.Dapper;

internal static class RepositoryRegistrar
{
    internal static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(provider =>
        {
            ILogger<ApplicationRepository> logger = provider.GetRequiredService<ILogger<ApplicationRepository>>();

            // TODO: use shared constant (currently in API project)
            string? connectionString = configuration.GetConnectionString("SQLConnection"); 

            IApplicationRepository applicationRepository = new ApplicationRepository(connectionString, logger);

            return applicationRepository;
        });
    }
}