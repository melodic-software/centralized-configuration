using Enterprise.API.Events;
using Microsoft.Extensions.DependencyInjection;
using Enterprise.Events.Services.Handling;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks;
using Enterprise.Events.Services.Raising.Callbacks.Facade;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Enterprise.API.Events.Decorators;
using Enterprise.DI.DotNet.Extensions;

namespace Configuration.DI.Registrars;

internal class EventServiceRegistrar
{
    internal static void RegisterEventServices(IServiceCollection services)
    {
        //services.BeginRegistration<IResolveEventHandlers>()
        //    .AddSingleton<EventHandlerResolver>()
        //    .WithDecorator<CachingEventHandlerResolver>();

        //services.BeginRegistration<IResolveEventHandlers>()
        //    .AddSingleton(provider => new EventHandlerResolver(provider))
        //    .WithDecorator((provider, eventHandlerResolver) => new CachingEventHandlerResolver(eventHandlerResolver));

        services.BeginRegistration<IResolveEventHandlers>()
            .AddSingleton(provider =>
            {
                IResolveEventHandlers eventHandlerResolver = new EventHandlerResolver(provider);
                return eventHandlerResolver;
            })
            .WithDecorator((provider, eventHandlerResolver) =>
            {
                // if this had additional dependencies, they could be resolved via the DI provider
                IResolveEventHandlers cachedEventHandlerResolver = new CachingEventHandlerResolver(eventHandlerResolver);
                return cachedEventHandlerResolver;
            });

        services.AddSingleton(provider =>
        {
            IResolveEventHandlers eventHandlerResolver = provider.GetRequiredService<IResolveEventHandlers>();
            IRaiseEvents eventRaiser = new EventRaiser(eventHandlerResolver);
            return eventRaiser;
        });

        // scoped lifetime services are created once per request within the scope
        // it is equivalent to a singleton in the current scope
        // for example, in MVC it creates one instance for each HTTP request, but it uses the same instance in the other calls within the same web request

        services.AddScoped(provider =>
        {
            IRegisterEventCallbacks eventCallbackRegistrar = new EventCallbackRegistrar();
            IRaiseEventCallbacks eventCallbackRaiser = new EventCallbackRaiser(eventCallbackRegistrar);

            IEventCallbackService eventCallbackService = new EventCallbackService(eventCallbackRegistrar, eventCallbackRaiser);

            return eventCallbackService;
        });
    }
}