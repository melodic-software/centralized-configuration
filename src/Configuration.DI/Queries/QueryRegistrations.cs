using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static Configuration.DI.Queries.Registrars.EntityFramework.PropertyMappingRegistrar;
using static Configuration.DI.Queries.Registrars.EntityFramework.QueryHandlerRegistrar;
using static Configuration.DI.Queries.Registrars.EntityFramework.RepositoryRegistrar;

namespace Configuration.DI.Queries;

public static class QueryRegistrations
{
    public static void RegisterQueryServices(this IServiceCollection services, IConfiguration configuration)
    {
        RegisterQueryHandlers(services);
        RegisterRepositories(services, configuration);
        RegisterPropertyMappings(services);
    }
}