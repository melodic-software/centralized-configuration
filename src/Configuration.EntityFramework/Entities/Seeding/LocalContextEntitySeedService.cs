using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Entities.Seeding;

public static class LocalContextEntitySeedService
{
    public static void SeedLocalContextEntities(ModelBuilder modelBuilder)
    {
        int id = 1;

        List<LocalContextEntity> localContextEntities = new List<LocalContextEntity>
        {
            new()
            {
                LocalContextId = id++,
                Identifier = "default",
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                LocalContextId = id,
                Identifier = "kyle.sexton@wachter.com",
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc)
            }
        };

        modelBuilder.Entity<LocalContextEntity>().HasData(localContextEntities);
    }
}