using System.Diagnostics;
using Configuration.ApplicationServices.Applications.Shared;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.Core.Queries.Paging;
using Enterprise.Core.Queries.Searching;
using Enterprise.Core.Queries.Sorting;
using Enterprise.DomainDrivenDesign.Events;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using static Configuration.ApplicationServices.Applications.Shared.ApplicationQueryConstants;

namespace Configuration.ApplicationServices.Applications.GetApplications;

public sealed class GetApplicationsHandler(
    IRaiseEvents eventRaiser,
    IEventCallbackService eventCallbackService,
    IValidateSort sortValidator,
    IApplicationRepository applicationRepository)
    : QueryHandler<GetApplications, GetApplicationsResult>(eventRaiser, eventCallbackService)
{
    public override async Task<GetApplicationsResult> HandleAsync(GetApplications query, CancellationToken cancellationToken)
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
        PagedList<ApplicationResult> pagedList = await applicationRepository.GetApplicationsAsync(filterOptions, searchOptions, pagingOptions, sortOptions, cancellationToken);
        Activity.Current?.AddEvent(new ActivityEvent("Retrieved applications from repository"));

        GetApplicationsResult queryResult = new GetApplicationsResult(pagedList, pagedList.PaginationMetadata);

        return queryResult;
    }
}