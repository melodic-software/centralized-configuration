using System.Collections;
using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.Events.Services.Raising.Callbacks.Facade;

/// <summary>
/// This is a simple facade service that aggregates the registration and raising of event callbacks.
/// </summary>
public class EventCallbackService(IRegisterEventCallbacks callbackRegistrar, IRaiseEventCallbacks callbackRaiser)
    : IEventCallbackService
{
    public Dictionary<Type, IList> GetRegisteredCallbacks()
    {
        return callbackRegistrar.GetRegisteredCallbacks();
    }

    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        callbackRegistrar.RegisterEventCallback(eventCallback);
    }

    public void ClearRegisteredCallbacks()
    {
        callbackRegistrar.ClearRegisteredCallbacks();
    }

    public void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        callbackRaiser.RaiseCallbacks(events);
    }

    public void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent
    {
        callbackRaiser.RaiseCallbacks((dynamic)@event, callbackRegistrar);
    }

    public void RaiseCallbacks<TEvent>(TEvent @event, IRegisterEventCallbacks callbackRegistrar) where TEvent : IEvent
    {
        callbackRaiser.RaiseCallbacks((dynamic)@event, callbackRegistrar);
    }
}