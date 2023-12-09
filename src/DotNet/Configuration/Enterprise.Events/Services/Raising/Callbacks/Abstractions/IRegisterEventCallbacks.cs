using Enterprise.Events.Model;
using System.Collections;

namespace Enterprise.Events.Services.Raising.Callbacks.Abstractions;

public interface IRegisterEventCallbacks
{
    public Dictionary<Type, IList> GetRegisteredCallbacks();
    public void RegisterEventCallback<TEvent>(Action<TEvent> eventCallback) where TEvent : IEvent;
    void ClearRegisteredCallbacks();
}