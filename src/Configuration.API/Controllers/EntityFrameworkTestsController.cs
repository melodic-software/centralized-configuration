using Configuration.API.Routing;
using Configuration.EntityFramework.DbContexts.Configuration;
using Configuration.EntityFramework.Entities;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Configuration.API.Controllers;

[Route(RouteTemplates.EntityFrameworkTests)]
[ApiController]
public class EntityFrameworkTestsController : CustomControllerBase
{
    private readonly ConfigurationContext _dbContext;

    public EntityFrameworkTestsController(ConfigurationContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost(Name = RouteNames.CreateEntityFrameworkTest)]
    public async Task<IActionResult> Post()
    {
        DbSet<ApplicationEntity> dbSet = _dbContext.Applications;

        IQueryable<ApplicationEntity> query = dbSet.AsQueryable();

        ApplicationEntity? applicationByDomainId =
            await query.FirstOrDefaultAsync(x => x.DomainId == new Guid("500f86a2-65f7-4fc2-836a-2b14f8686209"));

        ApplicationEntity? applicationByIdResult = await dbSet.FindAsync(applicationByDomainId?.ApplicationId);

        List<ApplicationEntity> allApplications = await query
            .OrderBy(x => x.IsActive)
            .ThenBy(x => x.Name)
            .ToListAsync();
            
        List<ApplicationEntity> likeResult = await query
            .Where(x => EF.Functions.Like(x.Name, "D%"))
            .ToListAsync();

        _dbContext.ChangeTracker.DetectChanges();
        string shortDebugView = _dbContext.ChangeTracker.DebugView.ShortView;

        return StatusCode(StatusCodes.Status201Created);
    }
}