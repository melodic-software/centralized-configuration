﻿using Configuration.ApplicationServices.Applications.GetApplications;
using Enterprise.Core.Queries.Paging;
using Enterprise.Core.Queries.Searching;
using Enterprise.Core.Queries.Sorting;

namespace Configuration.ApplicationServices.Applications.Shared;

public interface IApplicationRepository
{
    Task<PagedList<ApplicationResult>> GetApplicationsAsync(ApplicationFilterOptions filterOptions,
        SearchOptions searchOptions, PagingOptions pagingOptions, SortOptions sortOptions,
        CancellationToken cancellationToken);

    Task<ApplicationResult?> GetByUniqueNameAsync(string uniqueName, CancellationToken cancellationToken);
}