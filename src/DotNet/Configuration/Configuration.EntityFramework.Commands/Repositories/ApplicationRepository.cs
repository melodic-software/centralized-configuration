using Configuration.Core.Domain.Model.Entities;
using Configuration.Core.Domain.Services.Abstract.Repositories;
using Configuration.EntityFramework.DbContexts.Configuration;
using Configuration.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Configuration.EntityFramework.Commands.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly ConfigurationContext _context;
    private readonly ILogger _logger;

    public ApplicationRepository(ConfigurationContext context, ILoggerFactory loggerFactory)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));

        // you can specific logger categories by type (which includes namespace)
        _logger = loggerFactory.CreateLogger<ApplicationRepository>();

        // OR by using a custom named category
        //_logger = loggerFactory.CreateLogger("DataAccessLayer"); 
    }

    public async Task<Application?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting application in repository with ID of {id}", id);

        Stopwatch stopwatch = Stopwatch.StartNew();

        ApplicationEntity? entity = await _context.Applications
            .FirstOrDefaultAsync(x => x.DomainId == id && !x.IsDeleted);

        stopwatch.Stop();

        _logger.LogDebug("Querying application with {id} finished in {ticks} ticks", id, stopwatch.ElapsedTicks);

        Application? application = Map(entity);

        return application;
    }

    public async Task<Application?> GetByUniqueNameAsync(string uniqueName)
    {
        ApplicationEntity? entity = await _context.Applications
            .FirstOrDefaultAsync(x => x.UniqueName == uniqueName && !x.IsDeleted);

        string debugShortView = _context.ChangeTracker.DebugView.ShortView;

        Application? application = Map(entity);

        return application;
    }

    public async Task Save(Application application)
    {
        ApplicationEntity? entity = await _context.Applications.FirstOrDefaultAsync(x => x.DomainId == application.Id);

        if (entity == null)
        {
            entity = new ApplicationEntity
            {
                DomainId = application.Id,
                DateCreated = DateTime.UtcNow
            };
        }
            
        entity.UniqueName = application.UniqueName;
        entity.Name = application.Name;
        entity.AbbreviatedName = application.AbbreviatedName;
        entity.Description = application.Description;
        entity.IsActive = application.IsActive;
        entity.DateModified = DateTime.UtcNow;

        if (entity.ApplicationId <= 0)
        {
            await _context.Applications.AddAsync(entity);
        }
        else
        {
            _context.Applications.Update(entity);
        }

        await SaveChangesAsync();

    }

    public async Task DeleteApplicationAsync(Guid id)
    {
        ApplicationEntity? entity = await _context.Applications.FirstOrDefaultAsync(x => x.DomainId == id);

        if (entity == null)
            return;

        await SoftDeleteApplication(entity);
    }

    private async Task SoftDeleteApplication(ApplicationEntity entity)
    {
        entity.IsDeleted = true;

        _context.Update(entity);

        await SaveChangesAsync();
    }

    private void HardDeleteApplication(ApplicationEntity entity)
    {
        // alternative option here is to use the remove on the entity
        _context.Applications.Remove(entity);

        // a better alternative would be to call a stored procedure
        // the one benefit of using this is that an entity does not have to be loaded in first
        // NOTE: the stored proc does not exist, this is for demo purposes
        // depending on the domain model, and data model - a domain ID (GUID) or SQL ID (int) can be used as the input param
        //_context.Database.ExecuteSqlInterpolatedAsync($"dbo.usp_Application_Delete {entity.DomainId}");
    }

    private async Task<bool> SaveChangesAsync()
    {
        bool result = await _context.SaveChangesAsync() >= 0;

        return result;
    }

    private static Application? Map(ApplicationEntity? entity)
    {
        if (entity == null)
            return null;

        // TODO: can we move this out to auto mapper?
        Application application = new Application(
            entity.DomainId,
            entity.Name,
            entity.AbbreviatedName,
            entity.Description,
            entity.IsActive
        );

        return application;
    }
}