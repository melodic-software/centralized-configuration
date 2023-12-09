using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Entities.Seeding;

public static class EnvironmentEntitySeedService
{
    public static void SeedEnvironmentEntities(ModelBuilder modelBuilder)
    {
        int id = 1;

        List<EnvironmentEntity> environments = new List<EnvironmentEntity>
        {
            new()
            {
                EnvironmentId = id ++,
                DomainId = Guid.Parse("fc52da1b-439a-4ffa-90f9-a9f7f585deb9"),
                UniqueName = "Local-fc52da1b",
                DisplayName = "Local",
                AbbreviatedDisplayName = null,
                IsActive = true,
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc),
                DateModified = null
            },
            new()
            {
                EnvironmentId = id++,
                DomainId = Guid.Parse("69c89b3b-654c-4461-baad-bd4f115d8a11"),
                UniqueName = "Development-69c89b3b",
                DisplayName = "Development",
                AbbreviatedDisplayName = "Dev",
                IsActive = true,
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc),
                DateModified = null
            },
            new()
            {
                EnvironmentId = id++,
                DomainId = Guid.Parse("629e10af-8c7e-4b76-9e01-985e28a6a08c"),
                UniqueName = "Testing-629e10af",
                DisplayName = "Testing",
                AbbreviatedDisplayName = "Test",
                IsActive = true,
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc),
                DateModified = null
            },
            new()
            {
                EnvironmentId = id++,
                DomainId = Guid.Parse("105c1e10-8701-43ca-a457-cd01f14591e5"),
                UniqueName = "Staging-105c1e10",
                DisplayName = "Staging",
                AbbreviatedDisplayName = "Stage",
                IsActive = true,
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc),
                DateModified = null
            },
            new()
            {
                EnvironmentId = id++,
                DomainId = Guid.Parse("bf420f08-a3ec-4bf9-bb87-90dc04c4b6b9"),
                UniqueName = "Production-bf420f08",
                DisplayName = "Production",
                AbbreviatedDisplayName = "Prod",
                IsActive = true,
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc),
                DateModified = null
            },
            new()
            {
                EnvironmentId = id,
                DomainId = Guid.Parse("f2000413-1926-44f4-ba3f-19ca8b2da955"),
                UniqueName = "Training-f2000413",
                DisplayName = "Training",
                AbbreviatedDisplayName = "Train",
                IsActive = false,
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc),
                DateModified = null
            }
        };

        modelBuilder.Entity<EnvironmentEntity>().HasData(environments);
    }
}