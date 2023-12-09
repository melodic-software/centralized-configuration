using Configuration.API.Client.Models.Output.V1;

namespace Configuration.API.InMemory.Singletons;

public static class ConfigurationEntryData
{
    private static readonly Lazy<List<ConfigurationEntryModel>> Data = new(Initialize);

    public static List<ConfigurationEntryModel> ConfigurationEntries => Data.Value;

    private static List<ConfigurationEntryModel> Initialize()
    {
        List<ConfigurationEntryModel> result = new List<ConfigurationEntryModel>
        {
            new(
                "4c75734e-fce6-473d-99e5-a01243d16584",
                "application setting",
                null,
                null,
                null,
                null,
                null,
                "Internal API Url",
                "InternalApiUrl",
                "https://internal-api-dev.wachter.com/api",
                null
            ),
            new(
                "84aba662-aa9e-48e0-93cb-3ebf22bcac1b",
                "application setting",
                null,
                null,
                null,
                null,
                null,
                "Integration Metadata API Url",
                "IntegrationMetadataApiUrl",
                "https://integration-metadata-api-dev.wachter.com",
                null
            ),
            new(
                "84aba662-aa9e-48e0-93cb-3ebf22bcac1b",
                "feature toggle",
                null,
                null,
                null,
                null,
                null,
                "Survey PDF Migration Feature Enabled",
                "FeatureToggle.SurveyPdfMigration",
                true,
                null
            ),
            new(
                "dcd95eaf-42c9-4964-b43b-43af9577305b",
                "connection string",
                null,
                null,
                null,
                null,
                null,
                "ETHOS Database Connection String",
                "ETHOS",
                "data source=DEV-SQL1.wachter.local;initial catalog=Ethos;integrated security=SSPI;persist security info=False;packet size=4096",
                null
            )
        };

        return result;
    }
}