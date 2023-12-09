using Enterprise.Events.Model;

namespace Enterprise.Events.Services.Handling.Generic;

public interface IHandleEvent<in T> : IHandleEvent where T : IEvent
{
    Task HandleAsync(T @event);
}