using Configuration.Dapper.Commands.Services;
using Configuration.Domain.Applications;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Commands.Registrars.Dapper;

internal class DomainServiceRegistrar
{
    internal static void RegisterDomainServices(IServiceCollection services)
    {
        services.AddScoped(provider =>
        {
            IApplicationExistenceService applicationExistenceService = new ApplicationExistenceService();
            return applicationExistenceService;
        });
    }
}