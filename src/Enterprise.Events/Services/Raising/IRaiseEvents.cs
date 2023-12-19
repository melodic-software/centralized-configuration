using Enterprise.Events.Model;

namespace Enterprise.Events.Services.Raising;

public interface IRaiseEvents
{
    Task RaiseAsync(IEvent @event);
    Task RaiseAsync(IEnumerable<IEvent> events);
}