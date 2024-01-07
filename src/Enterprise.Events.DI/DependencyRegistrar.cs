using Enterprise.DI.DotNet.Extensions;
using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Events.DI;

public static class DependencyRegistrar
{
    public static void RegisterEventHandler<T>(this IServiceCollection services,
        Func<IServiceProvider, IHandleEvent<T>> factory,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient) where T : IEvent
    {
        services.BeginRegistration<IHandleEvent<T>>()
            .Add(factory, serviceLifetime);
    }
}