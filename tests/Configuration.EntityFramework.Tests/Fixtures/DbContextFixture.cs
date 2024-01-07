using Configuration.EntityFramework.DbContexts.Configuration;
using Enterprise.DateTimes.Current;
using Enterprise.DateTimes.Current.Abstract;
using Enterprise.Sqlite.Constants;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Configuration.EntityFramework.Tests.Fixtures;

// https://app.pluralsight.com/course-player?clipId=99a3cf5b-eda0-44f3-aab0-9dc8cdde685d

public class DbContextFixture : IDisposable
{
    // By making this a readonly property, tests can get the instances via property wrappers,
    // but they cannot set the properties to new instances
    private ConfigurationContext DbContext { get; }

    public DbContextFixture()
    {
        SqliteConnection connection = new SqliteConnection(ConnectionStrings.InMemory);

        connection.Open();

        // in memory db context (using Sqlite)
        DbContextOptionsBuilder<ConfigurationContext> optionsBuilder = new DbContextOptionsBuilder<ConfigurationContext>().UseSqlite(connection);

        ICurrentDateTimeService currentDateTimeService = new UtcDateTimeService();

        ConfigurationContext dbContext = new ConfigurationContext(optionsBuilder.Options, currentDateTimeService);

        dbContext.Database.Migrate();

        DbContext = dbContext;
    }

    public void Dispose()
    {
        // clean up the test context / setup code, if required
        DbContext.Dispose();
    }
}