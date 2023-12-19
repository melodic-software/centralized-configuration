using Microsoft.Extensions.DependencyInjection;
using static Configuration.DI.Queries.Registrars.PropertyMappingRegistrar;
using static Configuration.DI.Queries.Registrars.QueryHandlerRegistrar;
using static Configuration.DI.Queries.Registrars.RepositoryRegistrar;

namespace Configuration.DI.Queries;

public static class QueryRegistrations
{
    public static void RegisterQueryServices(this IServiceCollection services)
    {
        RegisterQueryHandlers(services);
        RegisterRepositories(services);
        RegisterPropertyMappings(services);
    }
}