using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Services.Raising;

public class EventRaiser : IRaiseEvents
{
    private readonly IResolveEventHandlers _eventHandlerResolver;
    private readonly ILogger<EventRaiser> _logger;

    public EventRaiser(IResolveEventHandlers eventHandlerResolver, ILogger<EventRaiser> logger)
    {
        _eventHandlerResolver = eventHandlerResolver ?? throw new ArgumentNullException(nameof(eventHandlerResolver));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task RaiseAsync(IEnumerable<IEvent> events)
    {
        foreach (IEvent @event in events)
            await RaiseAsync((dynamic)@event);
    }

    public async Task RaiseAsync(IEvent @event)
    {
        IEnumerable<IHandleEvent> eventHandlers = await _eventHandlerResolver.ResolveAsync((dynamic)@event);

        if (!eventHandlers.Any())
        {
            _logger.LogWarning($"No event handlers registered for event: {@event.GetType().Name}");
            return;
        }

        foreach (IHandleEvent eventHandler in eventHandlers)
        {
            try
            {
                await eventHandler.HandleAsync(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred during event handler execution for {@event.GetType().Name}");
                throw;
            }
        }
    }
}