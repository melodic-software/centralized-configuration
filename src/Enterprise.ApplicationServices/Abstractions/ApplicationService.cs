using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.ApplicationServices.Abstractions;

public abstract class ApplicationService : IApplicationService
{
    private readonly HashSet<Guid> _processedEventIds = new();

    protected readonly IRaiseEvents EventRaiser;
    protected readonly IEventCallbackService EventCallbackService;

    protected ApplicationService(IRaiseEvents eventRaiser, IEventCallbackService eventCallbackService)
    {
        EventRaiser = eventRaiser ?? throw new ArgumentNullException(nameof(eventRaiser));
        EventCallbackService = eventCallbackService ?? throw new ArgumentNullException(nameof(eventCallbackService));
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
        foreach (IEvent @event in events)
            await RaiseEventAsync(@event);
    }

    protected async Task RaiseEventAsync(IEvent @event)
    {
        if (_processedEventIds.Contains(@event.Id))
        {
            // We keep track of every unique event ID, so we can be sure that
            // event handlers and callbacks are only executed once per event occurrence.
            return;
        }

        await EventRaiser.RaiseAsync(@event);
        EventCallbackService.RaiseCallbacks(@event);

        _processedEventIds.Add(@event.Id);
    }
}