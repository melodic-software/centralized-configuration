using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Entities.Seeding;

public static class ConfigurationDataTypeEntitySeedService
{
    public static void SeedConfigurationDataTypeEntities(ModelBuilder modelBuilder)
    {
        int id = 1;

        List<ConfigurationDataTypeEntity> configurationDataTypeEntities = new List<ConfigurationDataTypeEntity>()
        {
            new()
            {
                ConfigurationValueDataTypeId = id++,
                Name = "String",
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                ConfigurationValueDataTypeId = id,
                Name = "Boolean",
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc)
            }
        };

        modelBuilder.Entity<ConfigurationDataTypeEntity>().HasData(configurationDataTypeEntities);
    }
}