using Enterprise.Events.Model;

namespace Enterprise.DomainDrivenDesign.Events.Abstract;

public abstract class DomainEvent : EventBase, IDomainEvent;