using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Services.Raising;

public class EventRaiser(IResolveEventHandlers eventHandlerResolver, ILogger<EventRaiser> logger) : IRaiseEvents
{
    private readonly IResolveEventHandlers _eventHandlerResolver = eventHandlerResolver ?? throw new ArgumentNullException(nameof(eventHandlerResolver));

    public async Task RaiseAsync(IEnumerable<IEvent> events)
    {
        foreach (IEvent @event in events)
            await RaiseAsync((dynamic)@event);
    }

    public async Task RaiseAsync(IEvent @event)
    {
        IEnumerable<IHandleEvent?> eventHandlers = await _eventHandlerResolver.ResolveAsync((dynamic)@event);

        if (!eventHandlers.Any())
            logger.LogWarning($"No event handlers registered for event: {@event.GetType().Name}");

        foreach (IHandleEvent? eventHandler in eventHandlers)
        {
            if (eventHandler == null)
                continue;

            try
            {
                await eventHandler.HandleAsync(@event);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during event handler execution.");
                throw;
            }
        }
    }
}