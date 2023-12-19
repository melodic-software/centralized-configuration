using Microsoft.EntityFrameworkCore;
using static Configuration.EntityFramework.Providers.InMemory.Constants.InMemoryProviderConstants;

namespace Configuration.EntityFramework.Providers.InMemory.Extensions;

public static class DbContextOptionsBuilderExtensions
{
    public static void UseInMemory(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(InMemoryDatabaseName);
    }
}