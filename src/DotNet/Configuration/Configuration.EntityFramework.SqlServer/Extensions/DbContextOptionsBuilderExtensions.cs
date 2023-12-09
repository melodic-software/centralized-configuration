using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework.SqlServer.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static void UseSqlServer(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration, string connectionStringConfigKey)
    {
        if (string.IsNullOrEmpty(connectionStringConfigKey))
            throw new ArgumentNullException(nameof(connectionStringConfigKey));
            
        // NOTE: environment variables will override all the other configuration sources
        string? connectionString = configuration.GetConnectionString(connectionStringConfigKey);
        string? assemblyName = typeof(DbContextOptionsBuilderExtensions).Assembly.GetName().Name;

        if (string.IsNullOrWhiteSpace(assemblyName))
            throw new Exception("Migration assembly name is invalid!");

        optionsBuilder.UseSqlServer(connectionString, b => ConfigureSqlServer(b, assemblyName));
    }

    private static void ConfigureSqlServer(SqlServerDbContextOptionsBuilder builder, string assemblyName)
    {
        builder.MigrationsAssembly(assemblyName)

            // built in connection resiliency - depends on codes returned by SQL server
            // https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
            .EnableRetryOnFailure();
    }
}