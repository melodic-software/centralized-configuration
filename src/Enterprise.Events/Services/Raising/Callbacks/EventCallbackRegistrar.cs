using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using System.Collections;

namespace Enterprise.Events.Services.Raising.Callbacks;

public class EventCallbackRegistrar : IRegisterEventCallbacks
{
    private readonly Dictionary<Type, IList> _callbackDictionary;

    public EventCallbackRegistrar()
    {
        _callbackDictionary = new Dictionary<Type, IList>();
    }

    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);

        bool noCallbacksRegistered = !_callbackDictionary.ContainsKey(eventType);

        if (noCallbacksRegistered)
        {
            List<Action<TEvent>> eventCallbacks = new List<Action<TEvent>> { eventCallback };
            _callbackDictionary.Add(eventType, eventCallbacks);
        }
        else
        {
            List<Action<TEvent>>? existingCallbackList = _callbackDictionary[eventType] as List<Action<TEvent>>;
            existingCallbackList?.Add(eventCallback);
        }
    }

    public Dictionary<Type, IList> GetRegisteredCallbacks()
    {
        return _callbackDictionary;
    }

    public void ClearRegisteredCallbacks()
    {
        _callbackDictionary.Clear();
    }
}