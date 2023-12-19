using Enterprise.Events.Model;

namespace Enterprise.Events.Services.Handling;

public interface IHandleEvent
{
    Task HandleAsync(IEvent @event);
}