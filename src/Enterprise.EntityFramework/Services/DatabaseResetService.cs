using Enterprise.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Services;

public static class DatabaseResetService
{
    public static async Task ResetDatabaseAsync<T>(WebApplication app) where T : DbContext
    {
        try
        {
            if (!app.Environment.IsLocal())
                return;

            using IServiceScope scope = app.Services.CreateScope();

            DbContext? context = scope.ServiceProvider.GetService<T>();

            if (context != null)
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while resetting the database.");
            throw;
        }
    }
}