﻿using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Entities.Seeding;

public class ConfigurationEntryTypeEntitySeedService
{
    public static void SeedConfigurationEntryTypeEntities(ModelBuilder modelBuilder)
    {
        int id = 1;

        List<ConfigurationEntryTypeEntity> configurationEntryTypeEntities = new List<ConfigurationEntryTypeEntity>()
        {
            new()
            {
                ConfigurationEntryTypeId = id++,
                Name = "ApplicationSetting",
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                ConfigurationEntryTypeId = id++,
                Name = "ConnectionString",
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc)
            },
            new()
            {
                ConfigurationEntryTypeId = id,
                Name = "FeatureToggle",
                DateCreated = new DateTime(2023, 9, 29, 23, 0, 0, DateTimeKind.Utc)
            }
        };

        modelBuilder.Entity<ConfigurationEntryTypeEntity>().HasData(configurationEntryTypeEntities);
    }
}