using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising.Callbacks.Abstractions;
using Enterprise.Events.Services.Raising.Constants;
using Microsoft.Extensions.Logging;
using System.Collections;

namespace Enterprise.Events.Services.Raising.Callbacks;

public class EventCallbackRaiser : IRaiseEventCallbacks
{
    private readonly IRegisterEventCallbacks _eventCallbackRegistrar;
    private readonly ILogger<EventCallbackRaiser> _logger;

    public EventCallbackRaiser(IRegisterEventCallbacks eventCallbackRegistrar, ILogger<EventCallbackRaiser> logger)
    {
        _eventCallbackRegistrar = eventCallbackRegistrar;
        _logger = logger;
    }

    public void RaiseCallbacks(IEnumerable<IEvent> events)
    {
        List<IEvent> eventList = events.ToList();

        _logger.LogInformation("Raising callbacks for {eventCount} events.", eventList.Count);

        foreach (IEvent @event in eventList)
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
        {
            _logger.LogInformation("No callbacks have been registered for {eventTypeName}", eventType.Name);
            return;
        }

        if (registeredCallbacks[eventType] is not List<Action<TEvent>> callbackList)
        {
            _logger.LogInformation("Callbacks have been incorrectly registered.");

            // We know at this point that we have callbacks registered.
            // Here we have a type mismatch for the collection type in the callback dictionary.
            throw new Exception(CallbackConstants.CallbackTypeMismatchErrorMessage);
        }

        _logger.LogInformation("Executing {callbackCount} callback(s)", callbackList.Count);

        foreach (Action<TEvent> action in callbackList)
            action.Invoke(@event);
    }
}