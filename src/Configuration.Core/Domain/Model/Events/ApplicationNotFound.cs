using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Configuration.Core.Domain.Model.Events;

public class ApplicationNotFound : DomainEvent
{
    public Guid Id { get; }

    public ApplicationNotFound(Guid id)
    {
        Id = id;
    }
}