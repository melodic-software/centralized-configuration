using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Abstractions;

public abstract class ApplicationService : IApplicationService
{
    private readonly HashSet<Guid> _processedEventIds = new();

    protected readonly IRaiseEvents EventRaiser;
    protected readonly IEventCallbackService EventCallbackService;

    private readonly ILogger<ApplicationService> _logger;

    protected ApplicationService(IRaiseEvents eventRaiser, IEventCallbackService eventCallbackService, ILogger<ApplicationService> logger)
    {
        EventRaiser = eventRaiser ?? throw new ArgumentNullException(nameof(eventRaiser));
        EventCallbackService = eventCallbackService ?? throw new ArgumentNullException(nameof(eventCallbackService));
        _logger = logger;
    }

    public void ClearCallbacks()
    {
        EventCallbackService.ClearRegisteredCallbacks();
    }

    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        EventCallbackService.RegisterEventCallback(eventCallback);
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

        await EventRaiser.RaiseAsync(@event);
        EventCallbackService.RaiseCallbacks(@event);

        _processedEventIds.Add(@event.Id);
    }
}