using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Configuration.Domain.Applications.Events;

public class ApplicationNotFound : DomainEvent
{
    public Guid ApplicationId { get; }

    public ApplicationNotFound(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}