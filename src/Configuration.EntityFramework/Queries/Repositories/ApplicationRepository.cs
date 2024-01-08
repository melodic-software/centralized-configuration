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
using Configuration.ApplicationServices.Applications.GetApplications;
using Configuration.ApplicationServices.Applications.Shared;

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

    public async Task<PagedList<ApplicationResult>> GetApplicationsAsync(ApplicationFilterOptions filterOptions, SearchOptions searchOptions, 
        PagingOptions pagingOptions, SortOptions sortOptions, CancellationToken cancellationToken)
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
                .GetPropertyMapping<ApplicationResult, ApplicationEntity>();

            // dynamic sort
            query = query.ApplySort(sortOptions.OrderBy, mappingDictionary);
        }

        PagedList<ApplicationResult> result = await query
            .ToPagedListAsync(pagingOptions, queryable => queryable.ToListAsync(cancellationToken), Map);

        return result;
    }

    public async Task<ApplicationResult?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting application in repository with ID: {id}", id);

        Stopwatch stopWatch = Stopwatch.StartNew();

        ApplicationEntity? entity = await _context.Applications.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.DomainId == id, cancellationToken);

        stopWatch.Stop();

        _logger.LogDebug("Querying application with {id} finished in {milliseconds} milliseconds",
            id, stopWatch.ElapsedMilliseconds);

        if (entity == null)
            return null;

        ApplicationResult application = Map(entity);

        return application;
    }

    public async Task<ApplicationResult?> GetByUniqueNameAsync(string uniqueName, CancellationToken cancellationToken)
    {
        ApplicationEntity? entity = await _context.Applications.AsNoTracking()
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.UniqueName == uniqueName, cancellationToken);

        if (entity == null)
            return null;

        ApplicationResult application = Map(entity);

        return application;
    }

    private static ApplicationResult Map(ApplicationEntity entity)
    {
        return new ApplicationResult(
            entity.DomainId, 
            entity.UniqueName, 
            entity.Name, 
            entity.AbbreviatedName,
            entity.Description, 
            entity.IsActive
        );
    }
}