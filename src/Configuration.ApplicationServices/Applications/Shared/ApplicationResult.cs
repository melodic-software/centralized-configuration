namespace Configuration.ApplicationServices.Applications.Shared;

public class ApplicationResult
{
    public Guid Id { get; set; }
    public string UniqueName { get; set; }
    public string Name { get; set; }
    public string? AbbreviatedName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public ApplicationResult(Guid id, string uniqueName, string name, string? abbreviatedName, string? description, bool isActive)
    {
        Id = id;
        UniqueName = uniqueName;
        Name = name;
        AbbreviatedName = abbreviatedName;
        Description = description;
        IsActive = isActive;
    }
}