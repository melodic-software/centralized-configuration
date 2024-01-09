using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Enterprise.DomainDrivenDesign.Events.Example;

public class UserCreatedDomainEvent : DomainEvent
{
    public UserCreatedDomainEvent(Guid userId)
    {
    }
}