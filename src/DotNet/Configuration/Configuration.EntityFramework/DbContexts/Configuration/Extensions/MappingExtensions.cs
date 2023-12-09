using Configuration.EntityFramework.Entities;
using Configuration.EntityFramework.Providers.Sqlite.Mapping;
using Configuration.EntityFramework.Providers.SqlServer.Mapping;
using Enterprise.EntityFramework.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Enterprise.EntityFramework.Services.TableNameService;

namespace Configuration.EntityFramework.DbContexts.Configuration.Extensions;

public static class MappingExtensions
{
    // check this out for customized / more complex many to many relationships (in code mapping)
    // https://app.pluralsight.com/course-player?clipId=5633a0c9-3b70-421d-87e4-98af8d9b96a2

    public static void Map(this ConfigurationContext dbContext, ModelBuilder modelBuilder)
    {
        MapShared(dbContext, modelBuilder);
        MapProviderSpecific(dbContext, modelBuilder);
    }

    private static void MapShared(ConfigurationContext dbContext, ModelBuilder modelBuilder)
    {
        // application
        EntityTypeBuilder<ApplicationEntity> appEntryBuilder = modelBuilder.Entity<ApplicationEntity>();
        appEntryBuilder.ToTable(GetTableName(nameof(ApplicationEntity)));
            
        // configuration data type
        EntityTypeBuilder<ConfigurationDataTypeEntity> configDataTypeBuilder = modelBuilder.Entity<ConfigurationDataTypeEntity>();
        configDataTypeBuilder.ToTable(GetTableName(nameof(ConfigurationDataTypeEntity)));

        // configuration entry type
        EntityTypeBuilder<ConfigurationEntryTypeEntity> configEntryTypeBuilder = modelBuilder.Entity<ConfigurationEntryTypeEntity>();
        configEntryTypeBuilder.ToTable(GetTableName(nameof(ConfigurationEntryTypeEntity)));

        // configuration entry
        EntityTypeBuilder<ConfigurationEntryEntity> configEntryBuilder = modelBuilder.Entity<ConfigurationEntryEntity>();
        configEntryBuilder.ToTable(GetTableName(nameof(ConfigurationEntryEntity)));

        // configuration value
        EntityTypeBuilder<ConfigurationValueEntity> configValueBuilder = modelBuilder.Entity<ConfigurationValueEntity>();
        configValueBuilder.ToTable(GetTableName(nameof(ConfigurationValueEntity)));

        // environment
        EntityTypeBuilder<EnvironmentEntity> environmentEntityBuilder = modelBuilder.Entity<EnvironmentEntity>();
        environmentEntityBuilder.ToTable(GetTableName(nameof(EnvironmentEntity)));

        // label
        EntityTypeBuilder<LabelEntity> labelEntityBuilder = modelBuilder.Entity<LabelEntity>();
        labelEntityBuilder.ToTable(GetTableName(nameof(LabelEntity)));

        // local context
        EntityTypeBuilder<LocalContextEntity> localContextEntityBuilder = modelBuilder.Entity<LocalContextEntity>();
        localContextEntityBuilder.ToTable(GetTableName(nameof(LocalContextEntity)));

        // if you want to group a set of entity properties under a complex type, but not store them in a separate table
        // you can map them as a complex type using the modelBuilder.Entity<T>().OwnsOnes(x => x.{PropertyName});
        // the related type must be keyless (no database primary key property or annotation)
        // by default EF core persists this as {PropertyName}_{ComplexTypePropertyName}
        // these can be used for value objects (DDD concept)
    }

    private static void MapProviderSpecific(ConfigurationContext dbContext, ModelBuilder modelBuilder)
    {
        switch (dbContext.Database.ProviderName)
        {
            case ProviderConstants.SqlLiteProviderName:
                SqliteProviderMappingService.ConfigureMappings(dbContext, modelBuilder);
                break;
            case ProviderConstants.SqlServerProviderName:
                SqlServerProviderMappingService.ConfigureMappings(dbContext, modelBuilder);
                break;
            case ProviderConstants.InMemoryProviderName:
                break;
            default:
                break;
        }
    }
}