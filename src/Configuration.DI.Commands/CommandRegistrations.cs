using Microsoft.Extensions.DependencyInjection;
using static Configuration.DI.Commands.Registrars.CommandHandlerRegistrar;
using static Configuration.DI.Commands.Registrars.DomainServiceRegistrar;
using static Configuration.DI.Commands.Registrars.EventHandlerRegistrar;
using static Configuration.DI.Commands.Registrars.RepositoryRegistrar;

namespace Configuration.DI.Commands;

public static class CommandRegistrations
{
    public static void RegisterCommandServices(this IServiceCollection services)
    {
        RegisterCommandHandlers(services);
        RegisterRepositories(services);
        RegisterDomainServices(services);
        RegisterEventHandlers(services);
    }
}