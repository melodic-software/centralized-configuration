using Configuration.Core.Queries.Filtering;
using Configuration.Core.Queries.Model;
using Configuration.Core.Queries.Repositories;
using Configuration.EntityFramework.DbContexts.Configuration;
using Configuration.EntityFramework.Entities;
using Enterprise.Core.Queries.Paging;
using Enterprise.Core.Queries.Paging.Extensions;
using Enterprise.Core.Queries.Searching;
using Enterprise.Core.Queries.Sorting;
using Enterprise.Mapping.Properties.Model;
using Enterprise.Mapping.Properties.Services.Abstract;
using Enterprise.Sorting.Dynamic.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Configuration.EntityFramework.Queries.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly ConfigurationContext _context;
    private readonly IPropertyMappingService _propertyMappingService;
    private readonly ILogger<ApplicationRepository> _logger;

    public ApplicationRepository(ConfigurationContext context, IPropertyMappingService propertyMappingService, ILogger<ApplicationRepository> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _propertyMappingService = propertyMappingService;
        _logger = logger;
    }

    public async Task<PagedList<Application>> GetApplicationsAsync(ApplicationFilterOptions filterOptions, SearchOptions searchOptions, 
        PagingOptions pagingOptions, SortOptions sortOptions)
    {
        // working with IQueryable<T> means we can sequentially build a query (expression tree of query commands)
        // execution is deferred until the query is iterated, a method like .ToList() is called, or an aggregate singleton query like "Count" is executed
        IQueryable<ApplicationEntity> query = _context.Applications.AsNoTracking();

        query = query.Where(x => !x.IsDeleted);

        // filtering

        if (filterOptions.IsActive.HasValue)
            query = query.Where(x => x.IsActive == filterOptions.IsActive);

        if (!string.IsNullOrWhiteSpace(filterOptions.Name))
        {
            string name = filterOptions.Name.Trim();
            query = query.Where(x => x.Name == name);
        }

        if (!string.IsNullOrWhiteSpace(filterOptions.AbbreviatedName))
        {
            string abbreviatedDisplayName = filterOptions.AbbreviatedName.Trim();
            query = query.Where(x => x.AbbreviatedName != null && x.AbbreviatedName == abbreviatedDisplayName);
        }
            
        // search query
        // this functionality is typically performed by full text search components like Lucene
        // but can also be much simpler

        if (!string.IsNullOrWhiteSpace(searchOptions.SearchQuery))
        {
            query = query.Where(x => 
                x.Name.Contains(searchOptions.SearchQuery) ||
                (x.AbbreviatedName != null && x.AbbreviatedName.Contains(searchOptions.SearchQuery))
            );
        }

        // NOTE: this same technique can be applied to filtering
        // don't forget that query models, or API model contracts could contain different field representations (ex: concatenated, calculated, etc.)
        // the property mapping dictionary provides the translation

        if (!string.IsNullOrWhiteSpace(sortOptions.OrderBy))
        {
            Dictionary<string, PropertyMappingValue> mappingDictionary = _propertyMappingService
                .GetPropertyMapping<Application, ApplicationEntity>();

            // dynamic sort
            query = query.ApplySort(sortOptions.OrderBy, mappingDictionary);
        }

        PagedList<Application> result = await query
            .ToPagedListAsync(pagingOptions, queryable => queryable.ToListAsync(), Map);

        return result;
    }

    public async Task<Application?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting application in repository with ID: {id}", id);

        Stopwatch stopWatch = Stopwatch.StartNew();

        ApplicationEntity? entity = await _context.Applications.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.DomainId == id);

        stopWatch.Stop();

        _logger.LogDebug("Querying application with {id} finished in {milliseconds} milliseconds",
            id, stopWatch.ElapsedMilliseconds);

        if (entity == null)
            return null;

        Application application = Map(entity);

        return application;
    }

    public async Task<Application?> GetByUniqueNameAsync(string uniqueName)
    {
        ApplicationEntity? entity = await _context.Applications.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.UniqueName == uniqueName);

        if (entity == null)
            return null;

        Application application = Map(entity);

        return application;
    }

    private static Application Map(ApplicationEntity entity)
    {
        return new Application(
            entity.DomainId, 
            entity.UniqueName, 
            entity.Name, 
            entity.AbbreviatedName,
            entity.Description, 
            entity.IsActive
        );
    }
}