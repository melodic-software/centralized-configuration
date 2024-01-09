using Enterprise.ApplicationServices.Events;
using Enterprise.Events.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices;

public abstract class ApplicationServiceBase : IApplicationService
{
    private readonly HashSet<Guid> _processedEventIds = new();
    private readonly ILogger<ApplicationServiceBase> _logger;

    protected IEventServiceFacade EventService { get; }

    protected ApplicationServiceBase(IEventServiceFacade eventService, ILogger<ApplicationServiceBase> logger)
    {
        EventService = eventService;
        _logger = logger;
    }

    public void ClearCallbacks()
    {
        EventService.ClearRegisteredCallbacks();
    }

    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        EventService.RegisterEventCallback(eventCallback);
    }

    protected async Task RaiseEventsAsync(IEnumerable<IEvent> events)
    {
        // https://www.jetbrains.com/help/resharper/2023.3/PossibleMultipleEnumeration.html
        List<IEvent> eventList = events.ToList();

        _logger.LogInformation("Raising {eventCount} event(s).", eventList.Count);

        foreach (IEvent @event in eventList)
            await RaiseEventAsync(@event);
    }

    protected async Task RaiseEventAsync(IEvent @event)
    {
        if (_processedEventIds.Contains(@event.Id))
        {
            // We keep track of every unique event ID, so we can be sure that
            // event handlers and callbacks are only executed once per event occurrence.
            _logger.LogInformation("Event with ID \"{id}\" has already been processed.", @event.Id);
            return;
        }

        await EventService.RaiseAsync(@event);
        EventService.RaiseCallbacks(@event);

        _processedEventIds.Add(@event.Id);
    }
}