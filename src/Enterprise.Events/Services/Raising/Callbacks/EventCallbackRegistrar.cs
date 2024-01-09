using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections;

namespace Enterprise.Events.Services.Raising.Callbacks;

public class EventCallbackRegistrar : IRegisterEventCallbacks
{
    private readonly ILogger<EventCallbackRegistrar> _logger;
    private readonly Dictionary<Type, IList> _callbackDictionary;

    public EventCallbackRegistrar(ILogger<EventCallbackRegistrar> logger)
    {
        _logger = logger;
        _callbackDictionary = new Dictionary<Type, IList>();
    }

    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent
    {
        Type eventType = typeof(TEvent);

        bool noCallbacksRegistered = !_callbackDictionary.ContainsKey(eventType);

        if (noCallbacksRegistered)
        {
            
            List<Action<TEvent>> eventCallbacks = [eventCallback];
            _callbackDictionary.Add(eventType, eventCallbacks);
        }
        else
        {
            List<Action<TEvent>>? existingCallbackList = _callbackDictionary[eventType] as List<Action<TEvent>>;
            existingCallbackList?.Add(eventCallback);
        }

        _logger.LogInformation("Callback successfully registered for {eventTypeName}.", eventType.Name);

        int totalCallbacksForEvent = _callbackDictionary[eventType].Count;
        int totalCallbacks = _callbackDictionary.Sum(x => x.Value.Count);

        _logger.LogInformation("Total callbacks for event: {totalCallbacksForEvent}", totalCallbacksForEvent);
        _logger.LogInformation("Total callbacks registered: {totalCallbacks}", totalCallbacks);
    }

    public Dictionary<Type, IList> GetRegisteredCallbacks()
    {
        return _callbackDictionary;
    }

    public void ClearRegisteredCallbacks()
    {
        _callbackDictionary.Clear();
        _logger.LogInformation("Callback(s) successfully cleared.");
    }
}