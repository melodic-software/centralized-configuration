using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling.Generic;

namespace Enterprise.Events.Services.Handling;

public interface IResolveEventHandlers
{
    Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent;
}