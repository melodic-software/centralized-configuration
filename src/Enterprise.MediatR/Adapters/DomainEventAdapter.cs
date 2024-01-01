using Enterprise.DomainDrivenDesign.Event.Abstract;

namespace Enterprise.MediatR.Adapters;

public class DomainEventAdapter<TDomainEvent>(TDomainEvent domainEvent) : DomainEvent
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; } = domainEvent;
}