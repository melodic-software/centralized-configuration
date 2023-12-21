using Enterprise.DI.DotNet.Dependencies;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.DI.DotNet.Extensions;

public static class ServiceCollectionExtensions
{
    public static RegistrationContext<TService> BeginRegistration<TService>(this IServiceCollection services)
        where TService : class
    {
        return new RegistrationContext<TService>(services);
    }
}