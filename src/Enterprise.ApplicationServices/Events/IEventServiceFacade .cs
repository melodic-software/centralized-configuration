using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.ApplicationServices.Events;

public interface IEventServiceFacade : IRaiseEvents, IEventCallbackService;