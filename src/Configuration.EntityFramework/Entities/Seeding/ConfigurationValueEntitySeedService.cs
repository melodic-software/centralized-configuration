using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Entities.Seeding;

public static class ConfigurationValueEntitySeedService
{
    public static void SeedConfigurationValueEntities(ModelBuilder modelBuilder)
    {
        List<ConfigurationValueEntity> configurationValueEntities = new List<ConfigurationValueEntity>();

        int id = 1;

        // TODO: add entities

        modelBuilder.Entity<ConfigurationValueEntity>().HasData(configurationValueEntities);
    }
}