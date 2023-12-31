using Enterprise.DomainDrivenDesign.Event.Abstract;

namespace Enterprise.DomainDrivenDesign.Event.Example;

public class UserCreatedDomainEvent(Guid userId) : DomainEvent
{
    
}