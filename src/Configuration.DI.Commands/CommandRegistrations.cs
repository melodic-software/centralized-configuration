using Configuration.EntityFramework.DbContexts.Configuration;
using Enterprise.DesignPatterns.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using static Configuration.DI.Commands.Registrars.CommandHandlerRegistrar;
using static Configuration.DI.Commands.Registrars.EntityFramework.DomainServiceRegistrar;
using static Configuration.DI.Commands.Registrars.EntityFramework.RepositoryRegistrar;
using static Configuration.DI.Commands.Registrars.EventHandlerRegistrar;

namespace Configuration.DI.Commands;

public static class CommandRegistrations
{
    public static void RegisterCommandServices(this IServiceCollection services)
    {
        RegisterCommandHandlers(services);
        RegisterRepositories(services);
        RegisterDomainServices(services);
        RegisterEventHandlers(services);

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ConfigurationContext>());
    }
}