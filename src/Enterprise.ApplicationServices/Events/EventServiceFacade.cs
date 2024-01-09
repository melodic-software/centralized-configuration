using System.Collections;
using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.ApplicationServices.Events
{
    /// <summary>
    /// EventServiceFacade centralizes event-related functionalities for ease of use.
    /// It wraps the event raising and callback handling, delegating to specific services.
    /// This approach simplifies event management in the application by providing a cleaner
    /// and more streamlined interface, reducing direct dependencies on multiple event services.
    /// </summary>

    public class EventServiceFacade : IEventServiceFacade
    {
        private readonly IRaiseEvents _eventRaiser;
        private readonly IEventCallbackService _eventCallbackService;

        public EventServiceFacade(IRaiseEvents eventRaiser, IEventCallbackService eventCallbackService)
        {
            _eventRaiser = eventRaiser;
            _eventCallbackService = eventCallbackService;
        }

        public async Task RaiseAsync(IEvent @event)
        {
            await _eventRaiser.RaiseAsync(@event);
        }

        public async Task RaiseAsync(IEnumerable<IEvent> events)
        {
            await _eventRaiser.RaiseAsync(events);
        }

        public Dictionary<Type, IList> GetRegisteredCallbacks()
        {
            return _eventCallbackService.GetRegisteredCallbacks();
        }

        public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
        {
            _eventCallbackService.RegisterEventCallback(eventCallback);
        }

        public void ClearRegisteredCallbacks()
        {
            _eventCallbackService.ClearRegisteredCallbacks();
        }

        public void RaiseCallbacks(IEnumerable<IEvent> events)
        {
            _eventCallbackService.RaiseCallbacks(events);
        }

        public void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _eventCallbackService.RaiseCallbacks(@event);
        }

        public void RaiseCallbacks<TEvent>(TEvent @event, IRegisterEventCallbacks callbackRegistrar) where TEvent : IEvent
        {
            _eventCallbackService.RaiseCallbacks(@event, callbackRegistrar);
        }
    }
}
