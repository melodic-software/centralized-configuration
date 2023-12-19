using Enterprise.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Services;

public static class DatabaseMigrationService
{
    public static async Task Migrate<T>(WebApplication app) where T : DbContext
    {
        try
        {
            if (!app.Environment.IsLocal())
                return;

            using IServiceScope scope = app.Services.CreateScope();
            T dbContext = scope.ServiceProvider.GetRequiredService<T>();
            await dbContext.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }
}