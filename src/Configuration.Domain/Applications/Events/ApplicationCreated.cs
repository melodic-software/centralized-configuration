using Enterprise.DomainDrivenDesign.Event.Abstract;

namespace Configuration.Domain.Applications.Events;

public class ApplicationCreated : DomainEvent
{
    public Guid ApplicationId { get; }
    public string UniqueName { get; }
    public string Name { get; }
    public string? AbbreviatedName { get; }
    public string? Description { get; }
    public bool IsActive { get; }

    public ApplicationCreated(Guid applicationId, string uniqueName, string name, string? abbreviatedName, string? description, bool isActive)
    {
        ApplicationId = applicationId;
        UniqueName = uniqueName;
        Name = name;
        AbbreviatedName = abbreviatedName;
        Description = description;
        IsActive = isActive;
    }
}