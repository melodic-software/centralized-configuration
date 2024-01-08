using Enterprise.Events.Services.Raising.Callbacks.Abstractions;

namespace Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

public interface IEventCallbackService : IRegisterEventCallbacks, IRaiseEventCallbacks;