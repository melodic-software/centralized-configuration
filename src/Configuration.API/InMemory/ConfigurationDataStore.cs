using Configuration.API.InMemory.Singletons;
using Configuration.API.Client.Models.Output.V1;

namespace Configuration.API.InMemory;

public static class ConfigurationDataStore
{
    public static List<ConfigurationEntryDto> ConfigurationEntries => ConfigurationEntryData.ConfigurationEntries;
    public static List<EnvironmentDto> Environments => EnvironmentData.Environments;
}