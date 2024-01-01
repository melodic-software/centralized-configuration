using Enterprise.Events.Model;
using MediatR;

namespace Enterprise.MediatR.Adapters;

public class EventAdapter<TEvent>(TEvent @event) : INotification where TEvent : IEvent
{
    public TEvent Event { get; } = @event;
}