using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling;
using Enterprise.Events.Services.Handling.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.API.Events;

public class EventHandlerResolver : IResolveEventHandlers
{
    private readonly IServiceProvider _serviceProvider;

    public EventHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent
    {
        IEnumerable<IHandleEvent<T>> handlers = _serviceProvider.GetServices<IHandleEvent<T>>();
        return Task.FromResult(handlers);
    }
}