using Configuration.API.InMemory.Singletons;
using Configuration.API.Client.Models.Output.V1;

namespace Configuration.API.InMemory;

public static class ConfigurationDataStore
{
    public static List<ConfigurationEntryModel> ConfigurationEntries => ConfigurationEntryData.ConfigurationEntries;
    public static List<EnvironmentModel> Environments => EnvironmentData.Environments;
}