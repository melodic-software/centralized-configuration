namespace Configuration.API.Client.Models.Output.V1;

/// <summary>
/// The model for a specific application environment.
/// </summary>
public class EnvironmentModel
{
    public Guid EnvironmentId { get; set; }
    public string EnvironmentUniqueName { get; set; } = null!;
    public string EnvironmentName { get; set; } = null!;
    public string? AbbreviatedEnvironmentName { get; set; }
    public bool IsActive { get; set; }

    public EnvironmentModel(Guid environmentId, string environmentUniqueName, string environmentName, string? abbreviatedEnvironmentName, bool isActive)
    {
        EnvironmentId = environmentId;
        EnvironmentUniqueName = environmentUniqueName;
        EnvironmentName = environmentName;
        AbbreviatedEnvironmentName = abbreviatedEnvironmentName;
        IsActive = isActive;
    }

    public EnvironmentModel()
    {

    }
}