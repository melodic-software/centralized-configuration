using Enterprise.Events.Model;
using Enterprise.Events.Services.Handling.Generic;
using Enterprise.MediatR.Adapters.Factory;
using Enterprise.MediatR.Adapters.Factory.Abstract;
using Enterprise.MediatR.Dependencies;
using MediatR;

namespace Enterprise.MediatR.Adapters;

/// <summary>
/// Adapts an <see cref="IHandleEvent{T}"/> to MediatR's <see cref="INotificationHandler{TNotification}"/>.
/// It delegates the handling of a wrapped <see cref="EventAdapter{T}"/> to the provided event handler.
/// This allows for keeping our core event handling classes free from direct MediatR dependencies.
/// NOTE: There is some magic in the DI layer that will break if the constructor changes.
/// Please coordinate and review changes if needed.
/// <see cref="IEventHandlerAdapterFactory"/>
/// <see cref="EventHandlerAdapterFactory{TEvent}"/>
/// <see cref="DependencyRegistrar"/>
/// </summary>
/// <typeparam name="T">The type of event the adapter handles.</typeparam>
public class EventHandlerAdapter<T>(IHandleEvent<T> eventHandler)
    : INotificationHandler<EventAdapter<T>> where T : IEvent
{
    public Task Handle(EventAdapter<T> adapter, CancellationToken cancellationToken)
    {
        return eventHandler.HandleAsync(adapter.Event);
    }
}