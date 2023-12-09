using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Configuration.EntityFramework.Sqlite.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static void UseSqlite(this DbContextOptionsBuilder optionsBuilder, IConfiguration configuration, string connectionStringConfigKey, string? migrationsAssemblyName = null)
    {
        if (string.IsNullOrEmpty(connectionStringConfigKey))
            throw new ArgumentNullException(nameof(connectionStringConfigKey));

        string? connectionString = configuration.GetConnectionString(connectionStringConfigKey);

        migrationsAssemblyName ??= typeof(DbContextOptionsBuilderExtensions).Assembly.GetName().Name;

        if (string.IsNullOrWhiteSpace(migrationsAssemblyName))
            throw new Exception("Migration assembly name is invalid!");

        // this will live in the application root
        // Sqlite works regardless of the operating system you're using
        optionsBuilder.UseSqlite(connectionString,
            b => b.MigrationsAssembly(migrationsAssemblyName)
        );
    }
}