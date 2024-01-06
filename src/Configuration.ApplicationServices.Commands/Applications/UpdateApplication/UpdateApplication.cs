using Enterprise.ApplicationServices.Commands.Model;

namespace Configuration.ApplicationServices.Commands.Applications.UpdateApplication;

public class UpdateApplication : ICommand
{
    public Guid Id { get; }
    public string Name { get; }
    public string? AbbreviatedName { get; }
    public string? Description { get; }
    public bool IsActive { get; }

    public UpdateApplication(Guid id, string name, string? abbreviatedName, string? description, bool isActive)
    {
        Id = id;
        Name = name;
        AbbreviatedName = abbreviatedName;
        Description = description;
        IsActive = isActive;
    }
}