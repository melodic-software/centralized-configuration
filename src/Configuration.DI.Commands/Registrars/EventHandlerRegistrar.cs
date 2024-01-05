using Configuration.Domain.Applications.Events;
using Configuration.EventHandlers.Applications;
using Enterprise.Events.Services.Handling.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Commands.Registrars;

internal static class EventHandlerRegistrar
{

    /// <summary>
    /// Registers all event handlers within the application.
    /// This should be called during the application startup to ensure all event handlers
    /// are correctly registered in the service container for dependency injection.
    /// </summary>
    /// <param name="services">The service collection to which event handlers will be added.</param>
    internal static void RegisterEventHandlers(IServiceCollection services)
    {
        services.AddTransient(provider =>
        {
            IHandleEvent<ApplicationCreated> eventHandler = new ApplicationCreatedEventHandler();

            return eventHandler;
        });
    }
}