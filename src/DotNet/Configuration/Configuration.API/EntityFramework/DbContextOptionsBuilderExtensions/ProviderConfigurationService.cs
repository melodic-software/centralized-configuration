using Configuration.EntityFramework.Providers.InMemory.Extensions;
using Configuration.EntityFramework.Sqlite.Extensions;
using Configuration.EntityFramework.SqlServer.Extensions;
using Microsoft.EntityFrameworkCore;
using static Configuration.API.Constants.ConnectionStringConstants;
using static Configuration.API.EntityFramework.Constants.EntityFrameworkConstants;

namespace Configuration.API.EntityFramework.DbContextOptionsBuilderExtensions;

public static class ProviderConfigurationService
{
    public static void ConfigureProvider(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration)
    {
        string? providerName = configuration.GetValue(ProviderNameConfigKeyName, SqlServerConfigKey);

        switch (providerName)
        {
            case InMemoryProviderName:
                optionsBuilder.UseInMemory();
                break;
            case SQLiteProviderName:
                optionsBuilder.UseSqlite(configuration, SqlLiteConfigKey);
                break;
            case SQLServerProviderName:
                optionsBuilder.UseSqlServer(configuration, SqlServerConfigKey);
                break;
            default:
                throw new Exception("Invalid Entity Framework provider configured!");
        }
    }
}