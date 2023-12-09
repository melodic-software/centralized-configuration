using Microsoft.EntityFrameworkCore;

namespace Configuration.API.EntityFramework.DbContextOptionsBuilderExtensions;

public static class BehaviorConfigurationService
{
    public static void ConfigureBehavior(this DbContextOptionsBuilder optionsBuilder)
    {
        // this sets the default query tracking behavior
        // by default the context instances will default to non tracked queries
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        //EnableLazyLoading(optionsBuilder);
    }

    private static void EnableLazyLoading(DbContextOptionsBuilder optionsBuilder)
    {
        // this is required for lazy loading
        // NOTE: all navigational properties on a single entity must be marked virtual for this to work
        optionsBuilder.UseLazyLoadingProxies();

        // be careful with access to properties, one command / single property load is OK
        // but be careful of the following:

        // var bookCount = author.Books.Count();
        // this does not just issue the count, it loads all related records in and does the count in memory

        // data binding a grid to lazy-loaded data results in
        // sending N+1 commands to the database as each related record is loaded into a grid row

        // lazy loading when no context is in scope
    }
}