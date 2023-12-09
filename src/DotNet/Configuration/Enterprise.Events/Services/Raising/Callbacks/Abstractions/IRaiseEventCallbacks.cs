using Enterprise.Events.Model;

namespace Enterprise.Events.Services.Raising.Callbacks.Abstractions;

public interface IRaiseEventCallbacks
{
    void RaiseCallbacks(IEnumerable<IEvent> events);
    void RaiseCallbacks<TEvent>(TEvent @event) where TEvent : IEvent;
    public void RaiseCallbacks<TEvent>(TEvent @event, IRegisterEventCallbacks callbackRegistrar) where TEvent : IEvent;
}