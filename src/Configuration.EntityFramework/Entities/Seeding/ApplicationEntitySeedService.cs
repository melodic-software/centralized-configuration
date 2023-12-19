using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Entities.Seeding;

public static class ApplicationEntitySeedService
{
    public static void SeedApplicationEntities(ModelBuilder modelBuilder)
    {
        int id = 1;

        List<ApplicationEntity> applicationEntities = new List<ApplicationEntity>
        {
            new()
            {
                ApplicationId = id++,
                DomainId = Guid.Parse("500f86a2-65f7-4fc2-836a-2b14f8686209"),
                UniqueName = "Demo Application-500f86a2",
                Name = "Demo Application",
                AbbreviatedName = "Demo App",
                Description = "This is a demo application.",
                IsActive = true,
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc),
                DateModified = null
            },
            new()
            {
                ApplicationId = id++,
                DomainId = Guid.Parse("a49262fd-9ab9-452e-92b9-bfb742c94bd0"),
                UniqueName = "Demo API-a49262fd",
                Name = "Demo API",
                AbbreviatedName = null,
                Description = null,
                IsActive = true,
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc),
                DateModified = null
            },
            new()
            {
                ApplicationId = id,
                DomainId = Guid.Parse("dd65fe33-97d0-4632-b7f4-677ffd2fdf14"),
                UniqueName = "Demo WinForm Application-dd65fe33",
                Name = "Demo WinForm Application",
                AbbreviatedName = null,
                Description = null,
                IsActive = false,
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc),
                DateModified = null
            },
        };

        modelBuilder.Entity<ApplicationEntity>().HasData(applicationEntities);
    }
}