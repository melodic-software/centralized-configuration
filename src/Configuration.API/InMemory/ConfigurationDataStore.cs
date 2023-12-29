using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.InMemory.Singletons;

namespace Configuration.API.InMemory;

public static class ConfigurationDataStore
{
    public static List<ConfigurationEntryDto> ConfigurationEntries => ConfigurationEntryData.ConfigurationEntries;
    public static List<EnvironmentDto> Environments => EnvironmentData.Environments;
}