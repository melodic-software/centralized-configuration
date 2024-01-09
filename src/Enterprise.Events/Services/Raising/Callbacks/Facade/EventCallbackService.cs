using System.Collections;
using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.Events.Services.Raising.Callbacks.Facade;

/// <summary>
/// This is a simple facade service that aggregates the registration and raising of event callbacks.
/// </summary>
public class EventCallbackService : IEventCallbackService
{
    private readonly IRegisterEventCallbacks _callbackRegistrar;
    private readonly IRaiseEventCallbacks _callbackRaiser;

    /// <summary>
    /// This is a simple facade service that aggregates the registration and raising of event callbacks.
    /// </summary>
    public EventCallbackService(IRegisterEventCallbacks callbackRegistrar, IRaiseEventCallbacks callbackRaiser)
    {
        _callbackRegistrar = callbackRegistrar;
        _callbackRaiser = callbackRaiser;
    }

    public Dictionary<Type, IList> GetRegisteredCallbacks()
    {
        return _callbackRegistrar.GetRegisteredCallbacks();
    }

    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        _callbackRegistrar.RegisterEventCallback(eventCallback);
    }

    public void ClearRegisteredCallbacks()
    {
        _callbackRegistrar.ClearRegisteredCallbacks();
    }

    public void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        _callbackRaiser.RaiseCallbacks(events);
    }

    public void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent
    {
        _callbackRaiser.RaiseCallbacks((dynamic)@event, _callbackRegistrar);
    }

    public void RaiseCallbacks<TEvent>(TEvent @event, IRegisterEventCallbacks callbackRegistrar) where TEvent : IEvent
    {
        _callbackRaiser.RaiseCallbacks((dynamic)@event, callbackRegistrar);
    }
}