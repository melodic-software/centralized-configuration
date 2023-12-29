namespace Configuration.API.Client.Models.Output.V1;

/// <summary>
/// The model contract for an application resource.
/// </summary>
public class ApplicationDto
{
    /// <summary>
    /// The unique identifier for an application.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The human readable unique name for an application.
    /// This is a combination of the name and the application ID.
    /// </summary>
    public string UniqueName { get; set; } = null!;

    /// <summary>
    /// The application name.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// The optional abbreviated name.
    /// </summary>
    public string? AbbreviatedName { get; set; }

    /// <summary>
    /// An optional description of the application.
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// The active status of an application.
    /// </summary>
    public bool IsActive { get; set; }

    public ApplicationDto(Guid id, string uniqueName, string name, string? abbreviatedName, string? description, bool isActive)
    {
        Id = id;
        UniqueName = uniqueName;
        Name = name;
        AbbreviatedName = abbreviatedName;
        Description = description;
        IsActive = isActive;
    }

    public ApplicationDto()
    {

    }
}