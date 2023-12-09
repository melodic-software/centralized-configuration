﻿using Configuration.Core.Domain.Model.Events;
using Enterprise.Events.Services.Handling.Generic;

namespace Configuration.EventHandlers.Applications;

public class ApplicationCreatedEventHandler : EventHandlerBase<ApplicationCreated>
{
    public override Task HandleAsync(ApplicationCreated @event)
    {
        return Task.CompletedTask;
    }
}