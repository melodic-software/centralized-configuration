using Configuration.EntityFramework.DbContexts.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Providers.SqlServer.Mapping;

public static class SqlServerProviderMappingService
{
    public static void ConfigureMappings(ConfigurationContext configurationContext, ModelBuilder modelBuilder)
    {
        // other methods
        // .Ignore()
        // .Ignore<TEntity>()
        // .HasIndex()
        // .AutoInclude() - eager load relational property by default
        // .HasConversion<T> (value converter)

        //modelBuilder.Entity<ApplicationEntity>()
        //    .Property(x => x.Description)
        //    .HasColumnName(nameof(ApplicationEntity.Description))
        //    .HasMaxLength(450)
        //    .IsRequired(false);
    }
}