using Enterprise.Events.Model;

namespace Enterprise.ApplicationServices;

public interface IApplicationService
{
    /// <summary>
    /// Clear any registered event callbacks.
    /// </summary>
    public void ClearCallbacks();

    /// <summary>
    /// Register a delegate to react to events that have been raised during use case execution.
    /// These are typically domain events.
    /// </summary>
    /// <typeparam name="TEvent">The type of event.</typeparam>
    /// <param name="eventCallback">The delegate to invoke when the event is raised.</param>
    void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent;
}