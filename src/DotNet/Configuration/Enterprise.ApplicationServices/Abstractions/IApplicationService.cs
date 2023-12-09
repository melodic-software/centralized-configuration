using Enterprise.Events.Model;

namespace Enterprise.ApplicationServices.Abstractions;

public interface IApplicationService
{
    /// <summary>
    /// Clear any registered event callbacks.
    /// </summary>
    public void ClearCallbacks();

    /// <summary>
    /// Register a handler delegate to respond to events that have been raised in the application service implementation.
    /// </summary>
    /// <typeparam name="TEvent">the type of event</typeparam>
    /// <param name="eventCallback">the delegate to invoke when the event is raised</param>
    void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent;
}