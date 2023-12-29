namespace Configuration.API.Client.Models.Output.V2;
// NOTE: this is just an example of a versioned model contract
// we should version model contracts if properties are removed, or renamed
// we can add properties (model extension) to the current version,
// but consider using a new model if there are multiple additions (increasing payload size)

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
    /// The active status of an application.
    /// </summary>
    public bool IsActive { get; set; }

    public ApplicationDto(Guid id, string uniqueName, string name, bool isActive)
    {
        Id = id;
        UniqueName = uniqueName;
        Name = name;
        IsActive = isActive;
    }

    public ApplicationDto()
    {

    }
}