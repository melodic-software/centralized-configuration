using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Configuration.Core.Domain.Model.Events;

public class ApplicationDeleted : DomainEvent
{
    public Guid ApplicationId { get; }
    public string UniqueName { get; }
    public string Name { get; }
    public string? AbbreviatedName { get; }

    public ApplicationDeleted(Guid applicationId, string uniqueName, string name, string? abbreviatedName)
    {
        ApplicationId = applicationId;
        UniqueName = uniqueName;
        Name = name;
        AbbreviatedName = abbreviatedName;
    }
}