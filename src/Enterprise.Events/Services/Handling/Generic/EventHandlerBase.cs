using Enterprise.Events.Model;

namespace Enterprise.Events.Services.Handling.Generic;

public abstract class EventHandlerBase<T> : IHandleEvent<T> where T : IEvent
{
    public async Task HandleAsync(IEvent @event)
    {
        await HandleAsync((dynamic)@event);
    }

    public abstract Task HandleAsync(T @event);
}