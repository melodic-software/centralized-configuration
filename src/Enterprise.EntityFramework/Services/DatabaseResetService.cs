using Enterprise.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.Services;

public static class DatabaseResetService
{
    public static async Task<bool> ResetDatabaseAsync<T>(WebApplication app) where T : DbContext
    {
        try
        {
            // This should ONLY ever run locally.
            if (!app.Environment.IsLocal())
                return false;

            using IServiceScope scope = app.Services.CreateScope();

            DbContext? dbContext = scope.ServiceProvider.GetService<T>();

            if (dbContext == null)
                return false;

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
         
            return true;
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while resetting the database.");
            throw;
        }
    }
}