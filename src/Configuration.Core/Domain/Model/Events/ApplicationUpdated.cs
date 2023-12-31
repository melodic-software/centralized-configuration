using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Configuration.Core.Domain.Model.Events;

public class ApplicationUpdated : DomainEvent
{
    public Guid ApplicationId { get; }
    public string UniqueName { get; }
    public string Name { get; }
    public string? AbbreviatedName { get; }
    public bool IsActive { get; }

    public ApplicationUpdated(Guid applicationId, string uniqueName, string name, string? abbreviatedName, bool isActive)
    {
        ApplicationId = applicationId;
        UniqueName = uniqueName;
        Name = name;
        AbbreviatedName = abbreviatedName;
        IsActive = isActive;
    }
}