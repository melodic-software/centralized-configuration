using Microsoft.EntityFrameworkCore;
using static Configuration.EntityFramework.Entities.Seeding.ApplicationEntitySeedService;
using static Configuration.EntityFramework.Entities.Seeding.ConfigurationDataTypeEntitySeedService;
using static Configuration.EntityFramework.Entities.Seeding.ConfigurationEntryTypeEntitySeedService;
using static Configuration.EntityFramework.Entities.Seeding.ConfigurationValueEntitySeedService;
using static Configuration.EntityFramework.Entities.Seeding.EnvironmentEntitySeedService;
using static Configuration.EntityFramework.Entities.Seeding.LocalContextEntitySeedService;

namespace Configuration.EntityFramework.DbContexts.Configuration.Seeding;

public static class SeedService
{
    public static void SeedData(ModelBuilder modelBuilder)
    {
        // NOTE: the "EnsureCreated" method will also use seeds applied with the "HasData" method

        SeedApplicationEntities(modelBuilder);
        SeedConfigurationDataTypeEntities(modelBuilder);
        SeedConfigurationEntryTypeEntities(modelBuilder);
        SeedConfigurationValueEntities(modelBuilder);
        SeedEnvironmentEntities(modelBuilder);
        SeedLocalContextEntities(modelBuilder);
    }
}