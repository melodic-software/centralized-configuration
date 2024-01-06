using Bogus;
using Configuration.EntityFramework.DbContexts.Configuration;
using Configuration.EntityFramework.Entities;
using Enterprise.Hosting.Extensions;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Configuration.Infrastructure.Data
{
    public static class FakeDataSeedService
    {
        public static async Task SeedFakeDataAsync(WebApplication app)
        {
            try
            {
                // This should NEVER run in production.
                if (app.Environment.IsProduction())
                    return;

                using IServiceScope scope = app.Services.CreateScope();

                ConfigurationContext? dbContext = scope.ServiceProvider.GetService<ConfigurationContext>();

                if (dbContext != null)
                {
                    await SeedFakeApplicationDataAsync(dbContext);
                }
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "An error occurred while seeding the database with fake data.");
                throw;
            }
        }

        private static async Task SeedFakeApplicationDataAsync(DbContext dbContext)
        {
            Faker faker = new Faker();

            List<ApplicationEntity> applications = new();

            for (int i = 0; i < 100; i++)
            {
                applications.Add(new ApplicationEntity()
                {
                    ApplicationId = i + 1,
                    DomainId = Guid.NewGuid(),
                    UniqueName = string.Empty,
                    Name = faker.Lorem.Text(),
                    AbbreviatedName = faker.Lorem.Word(),
                    Description = faker.Lorem.Sentence(3, 25),
                    IsActive = faker.Random.Bool(.9f),
                    IsDeleted = faker.Random.Bool(.1f),
                    DateCreated = faker.Date.Between(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow),
                    DateModified = null,
                    ConfigurationValues = new List<ConfigurationValueEntity>()
                });
            }

            foreach (ApplicationEntity application in applications)
            {
                string? domainIdPartial = application.DomainId.ToString()
                    .Split("-")
                    .FirstOrDefault();

                application.UniqueName = $"{application.Name}-{domainIdPartial}";

                application.DateModified = faker.Date
                    .Between(application.DateCreated, DateTime.UtcNow)
                    .OrNull(faker, .45f);

                await dbContext.AddAsync(application);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
