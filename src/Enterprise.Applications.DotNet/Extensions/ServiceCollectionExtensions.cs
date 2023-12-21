using Enterprise.Applications.DotNet.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Applications.DotNet.Extensions;

public static class ServiceCollectionExtensions
{
    public static RegistrationContext<TService> BeginRegistration<TService>(this IServiceCollection services)
        where TService : class
    {
        return new RegistrationContext<TService>(services);
    }
}