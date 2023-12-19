using Enterprise.ApplicationServices.Commands.Model;

namespace Configuration.ApplicationServices.Commands.Applications;

public class CreateApplication : ICommand
{
    public Guid? Id { get; }
    public string Name { get; }
    public string? AbbreviatedName { get; }
    public string? Description { get; }
    public bool? IsActive { get; }

    public CreateApplication(Guid? id, string name, string? abbreviateName, string? description, bool? isActive)
    {
        Id = id;
        Name = name;
        AbbreviatedName = abbreviateName;
        Description = description;
        IsActive = isActive;
    }
}