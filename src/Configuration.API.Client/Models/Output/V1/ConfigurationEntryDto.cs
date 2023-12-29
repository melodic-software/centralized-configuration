namespace Configuration.API.Client.Models.Output.V1;

/// <summary>
/// The model for a configuration entry resource.
/// </summary>
public class ConfigurationEntryDto
{
    public string ConfigurationEntryId { get; set; } = null!;


    /// <summary>
    /// This is the type of configuration entry?
    /// Examples: ApplicationSetting, ConnectionString, FeatureToggle (flag).
    /// </summary>
    public string ConfigurationEntryType { get; set; } = null!;

    /// <summary>
    /// An optional attribute that stores information about the data type of the value (string, boolean, etc.).
    /// This can be inspected and used to ensure applications process the value properly. 
    /// </summary>
    public string? DataType { get; set; }

    public string? EnvironmentId { get; set; }
    public string? EnvironmentUniqueName { get; set; }
    public string? ApplicationId { get; set; }
    public string? ApplicationUniqueName { get; set; }

    /// <summary>
    /// An optional descriptor used to differentiate configuration entries (outside of environment and application descriptors).
    /// Use labels as a way to create multiple versions of a configuration entry.
    /// For example, you can input an application version number or a Git commit ID in labels to identify key-values associated with a particular software build.
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// An optional display name for the configuration entry.
    /// If one is not provided, the key will be used.
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// Keys serve as case-sensitive, unicode-based string identifiers for key-value pairs and are referenced when storing and retrieving corresponding values.
    /// Hierarchical namespaces are created by using a ":" separator (by convention).
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// The configuration value.
    /// </summary>
    public object Value { get; set; } = null!;

    public ConfigurationEntryDto(string configurationEntryId, string configurationEntryType,
        string? environmentId, string? environmentUniqueName, string? applicationId, string? applicationUniqueName,
        string? label, string? displayName, string key, object value, string? dataType)
    {
        ConfigurationEntryId = configurationEntryId;
        ConfigurationEntryType = configurationEntryType;
        EnvironmentId = environmentId;
        EnvironmentUniqueName = environmentUniqueName;
        ApplicationId = applicationId;
        ApplicationUniqueName = applicationUniqueName;
        Label = label;
        DisplayName = displayName ?? key;
        Key = key;
        Value = value;
        DataType = dataType;
    }

    public ConfigurationEntryDto()
    {

    }
}