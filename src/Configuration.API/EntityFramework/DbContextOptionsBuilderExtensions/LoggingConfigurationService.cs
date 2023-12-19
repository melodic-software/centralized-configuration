using Enterprise.Hosting.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Configuration.API.EntityFramework.DbContextOptionsBuilderExtensions;

public static class LoggingConfigurationService
{
    public static void ConfigureLogging(this DbContextOptionsBuilder optionsBuilder, IHostEnvironment environment)
    {
        // EF Core logging extends .NET logging APIs

        // log to the console
        optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);

        // log to the debug window
        optionsBuilder.LogTo(log => Debug.WriteLine(log));

        // sensitive data logging
        if (environment.IsLocal() || environment.IsDevelopment())
        {
            // don't do this unless data has been cleansed OR if there is no sensitive data that can be accessed by the context
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}