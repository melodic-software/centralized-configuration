using Enterprise.Core.Queries.Paging;
using Enterprise.Core.Queries.Searching;
using Enterprise.Core.Queries.Sorting;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using Configuration.ApplicationServices.Applications.GetApplications;
using Configuration.ApplicationServices.Applications.Shared;

namespace Configuration.Dapper.Queries.Repositories;

public class ApplicationRepository(string? connectionString, ILogger<ApplicationRepository> logger) : IApplicationRepository
{
    private readonly IDbConnection _db = new SqlConnection(connectionString);
    private readonly ILogger<ApplicationRepository> _logger = logger;

    public Task<PagedList<ApplicationResult>> GetApplicationsAsync(ApplicationFilterOptions filterOptions, SearchOptions searchOptions,
        PagingOptions pagingOptions, SortOptions sortOptions, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationResult?> GetByUniqueNameAsync(string uniqueName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}