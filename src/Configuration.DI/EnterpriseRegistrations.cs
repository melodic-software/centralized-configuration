using Configuration.DI.Registrars;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI;

public static class EnterpriseRegistrations
{
    public static void RegisterEnterpriseServices(this IServiceCollection services)
    {
        ApplicationServiceRegistrar.RegisterApplicationServiceDependencies(services);
        EventServiceRegistrar.RegisterEventServices(services);
        ReflectionServiceRegistrar.RegisterReflectionServices(services);
    }
}