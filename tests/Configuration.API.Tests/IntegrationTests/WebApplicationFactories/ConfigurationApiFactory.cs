using Configuration.API.Tests.IntegrationTests.Loggers;
using Configuration.EntityFramework.DbContexts.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Configuration.API.Tests.IntegrationTests.WebApplicationFactories;
// https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests

public class ConfigurationApiFactory(ITestOutputHelper testOutputHelper) : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureLogging(loggingBuilder =>
        {
            loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(testOutputHelper));
        });

        builder.ConfigureServices(services =>
        {
            Type dbContextOptionsType = typeof(DbContextOptions<ConfigurationContext>);
            ServiceDescriptor? descriptor = services.SingleOrDefault(d => d.ServiceType == dbContextOptionsType);

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<ConfigurationContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDatabase");
            });

            // create a new service provider
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            using IServiceScope scope = serviceProvider.CreateScope();
            using ConfigurationContext dbContext = scope.ServiceProvider.GetRequiredService<ConfigurationContext>();

            dbContext.Database.EnsureCreated();
        });

        return base.CreateHost(builder);
    }
}