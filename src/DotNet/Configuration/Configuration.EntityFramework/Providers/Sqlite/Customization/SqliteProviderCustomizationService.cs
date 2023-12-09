using Configuration.EntityFramework.DbContexts.Configuration;
using Enterprise.EntityFramework.Providers.Sqlite.Services;
using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Providers.Sqlite.Customization;

public static class SqliteProviderCustomizationService
{
    public static void Customize(ConfigurationContext dbContext, ModelBuilder modelBuilder)
    {
        dbContext.AllowDateTimeOffsetSortingForSqlite(modelBuilder);
    }
}