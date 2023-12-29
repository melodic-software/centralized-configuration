using Configuration.API.Client.Models.Output.V1;

namespace Configuration.API.InMemory.Singletons;

public static class EnvironmentData
{
    private static readonly Lazy<List<EnvironmentDto>> Data = new(Initialize);

    public static List<EnvironmentDto> Environments => Data.Value;

    private static List<EnvironmentDto> Initialize()
    {
        List<EnvironmentDto> environmentModels = new List<EnvironmentDto>
        {
            new(
                new Guid("fc52da1b-439a-4ffa-90f9-a9f7f585deb9"),
                "Local-fc52da1b",
                "Local",
                null,
                true
            ),
            new(
                new Guid("69c89b3b-654c-4461-baad-bd4f115d8a11"),
                "Development-69c89b3b",
                "Development",
                "Dev",
                true
            ),
            new(
                new Guid("629e10af-8c7e-4b76-9e01-985e28a6a08c"),
                "Testing-629e10af",
                "Testing",
                "Test",
                true
            ),
            new(new Guid("105c1e10-8701-43ca-a457-cd01f14591e5"),
                "Staging-105c1e10",
                "Staging",
                "Stage",
                true
            ),
            new(
                new Guid("bf420f08-a3ec-4bf9-bb87-90dc04c4b6b9"),
                "Production-bf420f08",
                "Production",
                "Prod",
                true
            ),
            new(
                new Guid("f2000413-1926-44f4-ba3f-19ca8b2da955"),
                "Training-f2000413",
                "Training",
                "Train",
                false
            )
        };

        return environmentModels;
    }
}