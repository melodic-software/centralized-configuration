using Enterprise.ApplicationServices.Events;
using Enterprise.Events.Model;

namespace Enterprise.ApplicationServices;

public abstract class ApplicationServiceBase : IApplicationService
{
    private readonly HashSet<Guid> _processedEventIds = new();

    protected IEventServiceFacade EventService { get; }

    protected ApplicationServiceBase(IEventServiceFacade eventService)
    {
        EventService = eventService;
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
        await EventService.RaiseAsync(events);
    }

    protected async Task RaiseEventAsync(IEvent @event)
    {
        if (_processedEventIds.Contains(@event.Id))
            return;

        await EventService.RaiseAsync(@event);
        EventService.RaiseCallbacks(@event);

        _processedEventIds.Add(@event.Id);
    }
}