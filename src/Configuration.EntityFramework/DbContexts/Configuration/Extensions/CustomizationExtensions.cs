using Configuration.EntityFramework.Providers.Sqlite.Customization;
using Configuration.EntityFramework.Providers.SqlServer.Customization;
using Enterprise.EntityFramework.Constants;
using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.DbContexts.Configuration.Extensions;

public static class CustomizationExtensions
{
    public static void HandleProviderCustomizations(this ConfigurationContext dbContext, ModelBuilder modelBuilder)
    {
        switch (dbContext.Database.ProviderName)
        {
            case ProviderConstants.SqlLiteProviderName:
                SqliteProviderCustomizationService.Customize(dbContext, modelBuilder);
                break;
            case ProviderConstants.SqlServerProviderName:
                SqlServerProviderCustomizationService.Customize(dbContext, modelBuilder);
                break;
            case ProviderConstants.InMemoryProviderName:
                break;
            default:
                break;
        }
    }
}