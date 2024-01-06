using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling;
using Enterprise.Events.Services.Handling.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Applications.DotNet.Events;

public class EventHandlerResolver(IServiceProvider serviceProvider) : IResolveEventHandlers
{
    public Task<IEnumerable<IHandleEvent<T>>> ResolveAsync<T>(T @event) where T : IEvent
    {
        IEnumerable<IHandleEvent<T>> handlers = serviceProvider.GetServices<IHandleEvent<T>>();
        return Task.FromResult(handlers);
    }
}