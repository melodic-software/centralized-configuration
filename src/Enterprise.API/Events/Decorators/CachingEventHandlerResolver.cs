using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling.Generic;
using Enterprise.Events.Services.Handling;
using System.Collections.Concurrent;

namespace Enterprise.API.Events.Decorators;

public class CachingEventHandlerResolver(IResolveEventHandlers decoratedResolver) : IResolveEventHandlers
{
    private readonly ConcurrentDictionary<Type, IEnumerable<IHandleEvent>> _handlerCache = new();

    public async Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent
    {
        Type eventType = typeof(T);

        if (_handlerCache.TryGetValue(eventType, out IEnumerable<IHandleEvent>? cachedHandlers))
            return cachedHandlers.Cast<IHandleEvent<T>>();

        IEnumerable<IHandleEvent<T>> resolvedHandlers = await decoratedResolver.ResolveAsync(@event);
        cachedHandlers = resolvedHandlers.ToList();
        _handlerCache.TryAdd(eventType, cachedHandlers);

        return cachedHandlers.Cast<IHandleEvent<T>>();
    }
}