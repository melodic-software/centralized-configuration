using Enterprise.DomainDrivenDesign.Entities.Generic;

namespace Configuration.Domain.Applications;

public class Application : Entity<ApplicationId>
{
    public string UniqueName => GetUniqueName();
    public string Name { get; private set; }
    public string? AbbreviatedName { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    public Application(ApplicationId id, string name, string? abbreviatedName, string? description, bool isActive) : base(id)
    {
        Id = id;
        Name = name;
        AbbreviatedName = abbreviatedName;
        Description = description;
        IsActive = isActive;
    }

    public static Application New(Guid? id, string name, string? abbreviatedName, string? description, bool? isActive)
    {
        ApplicationId applicationId = id != null && id != Guid.Empty ? new ApplicationId(id.Value) : ApplicationId.New();

        // default to inactive if not provided
        isActive ??= false;

        Application application = new Application(applicationId, name, abbreviatedName, description, isActive.Value);

        return application;
    }

    public void Update(string name, string? abbreviatedName, string? description, bool isActive)
    {
        string uniqueName = UniqueName;

        bool nameChanged = Name != name;
        bool abbreviatedNameChanged = AbbreviatedName != abbreviatedName;
        bool descriptionChanged = Description != description;
        bool isActiveChanged = IsActive != isActive;

        if (nameChanged)
        {
            Name = name;
            // this causes the unique name to be updated
        }

        if (abbreviatedNameChanged)
        {
            AbbreviatedName = abbreviatedName;
        }

        if (descriptionChanged)
        {
            Description = description;
        }

        if (isActiveChanged)
        {
            IsActive = isActive;
        }

        bool uniqueNameChanged = uniqueName != UniqueName;

        if (uniqueNameChanged)
        {

        }
    }

    private string GetUniqueName()
    {
        string id = Id.ToString();
        int indexOf = id.IndexOf('-');

        string uniqueName = string.Empty;

        if (indexOf <= -1)
            return uniqueName;

        string firstSectionOfGuid = id.Substring(0, indexOf);
        uniqueName = $"{Name}-{firstSectionOfGuid}";

        return uniqueName;
    }
}