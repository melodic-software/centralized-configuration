using Configuration.Domain.Applications.Events;
using Enterprise.Events.Services.Handling.Generic;

namespace Configuration.ApplicationServices.Applications.CreateApplication;

public class ApplicationCreatedEventHandler : EventHandlerBase<ApplicationCreated>
{
    public override Task HandleAsync(ApplicationCreated @event)
    {
        return Task.CompletedTask;
    }
}