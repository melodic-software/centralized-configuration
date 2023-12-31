using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Enterprise.DomainDrivenDesign.Events.Example;

public class UserCreatedDomainEvent(Guid userId) : DomainEvent
{
    
}