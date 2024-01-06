using Configuration.Core.Queries.Filtering;
using Configuration.Core.Queries.Model;
using Enterprise.Core.Queries.Paging;
using Enterprise.Core.Queries.Searching;
using Enterprise.Core.Queries.Sorting;

namespace Configuration.Core.Queries.Repositories;

public interface IApplicationRepository
{
    Task<PagedList<Application>> GetApplicationsAsync(ApplicationFilterOptions filterOptions,
        SearchOptions searchOptions, PagingOptions pagingOptions, SortOptions sortOptions,
        CancellationToken cancellationToken);

    Task<Application?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Application?> GetByUniqueNameAsync(string uniqueName, CancellationToken cancellationToken);
}