using Configuration.API.EntityFramework.DbContextOptionsBuilderExtensions;
using Configuration.EntityFramework.DbContexts.Configuration;
using Configuration.Infrastructure.Data;
using Enterprise.EntityFramework.Services;
using Enterprise.Hosting.Extensions;
using static Configuration.API.EntityFramework.Constants.EntityFrameworkConstants;

namespace Configuration.API.EntityFramework;

public static class EntityFrameworkConfiguration
{
    public static void ConfigureDbContexts(IServiceCollection services, WebApplicationBuilder builder)
    {
        // default lifetime is "scoped"
        // pooling can help with performance where there are many short-lived transactions
        // this also pools connection and other database resources
        // using "AddDbContext" will result in new context instances created for every request to the API
        // https://learn.microsoft.com/en-us/ef/core/performance/advanced-performance-topics
        services.AddDbContextPool<ConfigurationContext>(optionsBuilder =>
        {
            optionsBuilder.ConfigureProvider(builder.Configuration);
            optionsBuilder.ConfigureLogging(builder.Environment);
            optionsBuilder.ConfigureBehavior();
            optionsBuilder.ConfigureInterceptors(builder.Services);
        });
    }

    public static void AddDbContextHealthChecks(IHealthChecksBuilder builder)
    {
        builder.AddDbContextCheck<ConfigurationContext>();
    }

    public static async Task OnRequestPipelineConfigured(WebApplication app)
    {
        if (app.Environment.IsLocal())
        {
            bool resetDatabase = app.Configuration.GetValue(ResetDatabaseConfigKeyName, false);

            if (resetDatabase)
            {
                // This deletes the database & migrates on startup, so we can start with a clean slate.
                bool databaseReset = await DatabaseResetService.ResetDatabaseAsync<ConfigurationContext>(app);

                // We should only do this once. It is safe to do if we are completely resetting the database.
                if (databaseReset)
                    await FakeDataSeedService.SeedFakeDataAsync(app);
            }
        }
        else if (!app.Environment.IsProduction())
        {
            // We can apply migrations at runtime.
            // We do NOT run migrations automatically in production.
            // https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying#apply-migrations-at-runtime
            await DatabaseMigrationService.Migrate<ConfigurationContext>(app);
        }
    }
}