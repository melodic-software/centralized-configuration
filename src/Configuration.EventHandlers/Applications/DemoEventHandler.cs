using Configuration.Domain.Applications.Events;
using Enterprise.Events.Services.Handling.Generic;

namespace Configuration.EventHandlers.Applications
{
    public class DemoEventHandler : EventHandlerBase<ApplicationCreated>
    {
        public override Task HandleAsync(ApplicationCreated @event)
        {
            return Task.CompletedTask;
        }
    }
}