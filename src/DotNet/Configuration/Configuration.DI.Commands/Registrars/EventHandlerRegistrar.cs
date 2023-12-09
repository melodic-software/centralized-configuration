using Configuration.Core.Domain.Model.Events;
using Configuration.EventHandlers.Applications;
using Enterprise.Events.Services.Handling.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration.DI.Commands.Registrars;

internal static class EventHandlerRegistrar
{
    internal static void RegisterEventHandlers(IServiceCollection services)
    {
        services.AddTransient(provider =>
        {
            IHandleEvent<ApplicationCreated> eventHandler = new ApplicationCreatedEventHandler();
            return eventHandler;
        });
    }
}