using Enterprise.Applications.DotNet.Dependencies.Registrars;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Applications.DotNet.Dependencies;

public static class EnterpriseRegistrations
{
    public static void RegisterEnterpriseServices(this IServiceCollection services)
    {
        ApplicationServiceRegistrar.RegisterApplicationServiceDependencies(services);
        EventServiceRegistrar.RegisterEventServices(services);
        ReflectionServiceRegistrar.RegisterReflectionServices(services);
        TemporalityServiceRegistrar.RegisterTemporalityServices(services);
    }
}