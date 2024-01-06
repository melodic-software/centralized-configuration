using Microsoft.Extensions.DependencyInjection;
using static Enterprise.Applications.DotNet.Dependencies.Registrars.ApplicationServiceRegistrar;
using static Enterprise.Applications.DotNet.Dependencies.Registrars.EventServiceRegistrar;
using static Enterprise.Applications.DotNet.Dependencies.Registrars.ReflectionServiceRegistrar;
using static Enterprise.Applications.DotNet.Dependencies.Registrars.TemporalityServiceRegistrar;

namespace Enterprise.Applications.DotNet.Dependencies;

public static class EnterpriseRegistrations
{
    public static void RegisterEnterpriseServices(this IServiceCollection services)
    {
        RegisterApplicationServiceDependencies(services);
        RegisterEventServices(services);
        RegisterReflectionServices(services);
        RegisterTemporalityServices(services);
    }
}