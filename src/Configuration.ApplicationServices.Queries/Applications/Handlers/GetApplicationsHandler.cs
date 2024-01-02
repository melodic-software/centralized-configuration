using Configuration.ApplicationServices.Queries.Applications.Results;
using Configuration.Core.Queries.Filtering;
using Configuration.Core.Queries.Model;
using Configuration.Core.Queries.Repositories;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.Core.Queries.Paging;
using Enterprise.Core.Queries.Searching;
using Enterprise.Core.Queries.Sorting;
using System.Diagnostics;
using Enterprise.DomainDrivenDesign.Event;
using static Configuration.Core.Queries.Constants.ApplicationQueryConstants;

namespace Configuration.ApplicationServices.Queries.Applications.Handlers;

public sealed class GetApplicationsHandler(
    IApplicationServiceDependencies appServiceDependencies,
    IValidateSort sortValidator,
    IApplicationRepository applicationRepository)
    : QueryHandler<GetApplications, GetApplicationsResult>(appServiceDependencies)
{
    public override async Task<GetApplicationsResult> HandleAsync(GetApplications query)
    {
        // TODO: this is where security / permission checks will go
        // inject in a service that performs these checks (claim data?)

        ApplicationFilterOptions filterOptions = new ApplicationFilterOptions(query.Name, query.AbbreviatedName, query.IsActive);
        SearchOptions searchOptions = new SearchOptions(query.SearchQuery);
        PagingOptions pagingOptions = new PagingOptions(query.PageNumber, query.PageSize, DefaultPageSize, MaxPageSize);
        SortOptions sortOptions = new SortOptions(query.OrderBy);

        ValidationFailure? sortValidationFailure = sortValidator.Validate(sortOptions);

        if (sortValidationFailure != null)
        {
            await RaiseEventAsync(sortValidationFailure);
            return GetApplicationsResult.Empty(pagingOptions);
        }

        Activity.Current?.AddEvent(new ActivityEvent("Getting applications from repository"));
        PagedList<Application> pagedList = await applicationRepository.GetApplicationsAsync(filterOptions, searchOptions, pagingOptions, sortOptions);
        Activity.Current?.AddEvent(new ActivityEvent("Retrieved applications from repository"));

        GetApplicationsResult queryResult = new GetApplicationsResult(pagedList, pagedList.PaginationMetadata);

        return queryResult;
    }
}