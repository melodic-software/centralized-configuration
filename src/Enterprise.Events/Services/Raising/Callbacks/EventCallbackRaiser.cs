using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Enterprise.Events.Services.Raising.Constants;
using System.Collections;

namespace Enterprise.Events.Services.Raising.Callbacks;

public class EventCallbackRaiser : IRaiseEventCallbacks
{
    private readonly IRegisterEventCallbacks _eventCallbackRegistrar;

    public EventCallbackRaiser(IRegisterEventCallbacks eventCallbackRegistrar)
    {
        _eventCallbackRegistrar = eventCallbackRegistrar;
    }

    public void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        foreach (IEvent @event in events)
            RaiseCallbacks((dynamic)@event);
    }

    public void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent
    {
        RaiseCallbacks((dynamic)@event, _eventCallbackRegistrar);
    }

    public void RaiseCallbacks<TEvent>(TEvent @event, IRegisterEventCallbacks callbackRegistrar) where TEvent : IEvent
    {
        Dictionary<Type, IList> registeredCallbacks = callbackRegistrar.GetRegisteredCallbacks();

        Type eventType = @event.GetType();
        bool callbacksRegistered = registeredCallbacks.ContainsKey(eventType);

        if (!callbacksRegistered)
            return;

        if (registeredCallbacks[eventType] is not List<Action<TEvent>> callbackList)
        {
            // we know at this point that we have callbacks registered
            // so if this is null, we have a type mismatch for the collection type in the callback dictionary
            // it might be helpful to have some logging visibility here, but an exception will do for now
            throw new Exception(CallbackConstants.CallbackTypeMismatchErrorMessage);
        }

        foreach (Action<TEvent> action in callbackList)
            action.Invoke(@event);
    }
}