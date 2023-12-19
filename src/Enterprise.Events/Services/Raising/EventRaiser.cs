using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling;

namespace Enterprise.Events.Services.Raising;

public class EventRaiser : IRaiseEvents
{
    private readonly IResolveEventHandlers _eventHandlerResolver;

    public EventRaiser(IResolveEventHandlers eventHandlerResolver)
    {
        _eventHandlerResolver = eventHandlerResolver ?? throw new ArgumentNullException(nameof(eventHandlerResolver));
    }

    public async Task RaiseAsync(IEnumerable<IEvent> events)
    {
        foreach (IEvent @event in events)
            await RaiseAsync((dynamic)@event);
    }

    public async Task RaiseAsync(IEvent @event)
    {
        IEnumerable<IHandleEvent> eventHandlers = await _eventHandlerResolver.ResolveAsync((dynamic)@event);

        // TODO: we probably want to know if we don't have any event handlers for a given event?
        // maybe inject a service that is configured to notify if we don't have handlers for specific events registered?

        foreach (IHandleEvent eventHandler in eventHandlers)
        {
            if (eventHandler == null)
                continue;

            await eventHandler.HandleAsync(@event);
        }
    }
}