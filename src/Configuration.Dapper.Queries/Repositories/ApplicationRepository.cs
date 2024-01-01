using System.Data;
using Configuration.Core.Queries.Filtering;
using Configuration.Core.Queries.Model;
using Configuration.Core.Queries.Repositories;
using Enterprise.Core.Queries.Paging;
using Enterprise.Core.Queries.Searching;
using Enterprise.Core.Queries.Sorting;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Dapper;

namespace Configuration.Dapper.Queries.Repositories;

public class ApplicationRepository(string? connectionString, ILogger<ApplicationRepository> logger) : IApplicationRepository
{
    private readonly IDbConnection _db = new SqlConnection(connectionString);
    private readonly ILogger<ApplicationRepository> _logger = logger;

    public Task<PagedList<Application>> GetApplicationsAsync(ApplicationFilterOptions filterOptions, SearchOptions searchOptions,
        PagingOptions pagingOptions, SortOptions sortOptions)
    {
        throw new NotImplementedException();
    }

    public async Task<Application?> GetByIdAsync(Guid id)
    {
        DynamicParameters dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("ApplicationGuid", id);

        dynamic? result = await _db
            .QueryFirstOrDefaultAsync("dbo.usp_GetApplicationById", dynamicParameters, commandType: CommandType.StoredProcedure);

        if (result == null)
            return null;

        string uniqueName = result.UniqueName;
        string applicationName = result.ApplicationName;
        string abbreviatedName = result.AbbreviatedName;
        string? description = result.ApplicationDescription;
        bool isActive = result.IsActive;

        Application application = new Application(id, uniqueName, applicationName, abbreviatedName, description, isActive);

        return application;
    }

    public Task<Application?> GetByUniqueNameAsync(string uniqueName)
    {
        throw new NotImplementedException();
    }
}