using Enterprise.API.Events;
using Microsoft.Extensions.DependencyInjection;
using Enterprise.Events.Services.Handling;
using Enterprise.Events.Services.Raising.Callbacks;
using Enterprise.Events.Services.Raising.Callbacks.Facade;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Enterprise.Events.Services.Raising;

namespace Configuration.DI.Registrars;

internal class EventServiceRegistrar
{
    internal static void RegisterEventServices(IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            IResolveEventHandlers eventHandlerResolver = new EventHandlerResolver(provider);
            return eventHandlerResolver;
        });

        services.AddSingleton(provider =>
        {
            IResolveEventHandlers eventHandlerResolver = provider.GetRequiredService<IResolveEventHandlers>();
            IRaiseEvents eventRaiser = new EventRaiser(eventHandlerResolver);
            return eventRaiser;
        });

        // Scoped lifetime services are created once per request within the scope.
        // It is equivalent to a singleton in the current scope.
        // For example, in MVC it creates one instance for each HTTP request, but it uses the same instance in the other calls within the same web request.

        services.AddScoped(provider =>
        {
            IRegisterEventCallbacks eventCallbackRegistrar = new EventCallbackRegistrar();
            IRaiseEventCallbacks eventCallbackRaiser = new EventCallbackRaiser(eventCallbackRegistrar);

            IEventCallbackService eventCallbackService = new EventCallbackService(eventCallbackRegistrar, eventCallbackRaiser);

            return eventCallbackService;
        });
    }
}