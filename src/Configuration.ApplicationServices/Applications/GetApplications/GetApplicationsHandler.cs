using Configuration.ApplicationServices.Applications.Shared;
using Enterprise.ApplicationServices.Events;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.Core.Queries.Paging;
using Enterprise.Core.Queries.Searching;
using Enterprise.Core.Queries.Sorting;
using Enterprise.DomainDrivenDesign.Events;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static Configuration.ApplicationServices.Applications.Shared.ApplicationQueryConstants;

namespace Configuration.ApplicationServices.Applications.GetApplications;

public sealed class GetApplicationsHandler : QueryHandlerBase<GetApplications, GetApplicationsResult>
{
    private readonly IValidateSort _sortValidator;
    private readonly IApplicationRepository _applicationRepository;

    public GetApplicationsHandler(IEventServiceFacade eventServiceFacade,
        ILogger<GetApplicationsHandler> logger,
        IValidateSort sortValidator,
        IApplicationRepository applicationRepository) : base(eventServiceFacade, logger)
    {
        _sortValidator = sortValidator;
        _applicationRepository = applicationRepository;
    }

    public override async Task<GetApplicationsResult> HandleAsync(GetApplications query, CancellationToken cancellationToken)
    {
        // TODO: this is where security / permission checks will go
        // inject in a service that performs these checks (claim data?)

        ApplicationFilterOptions filterOptions = new ApplicationFilterOptions(query.Name, query.AbbreviatedName, query.IsActive);
        SearchOptions searchOptions = new SearchOptions(query.SearchQuery);
        PagingOptions pagingOptions = new PagingOptions(query.PageNumber, query.PageSize, DefaultPageSize, MaxPageSize);
        SortOptions sortOptions = new SortOptions(query.OrderBy);

        ValidationFailure? sortValidationFailure = _sortValidator.Validate(sortOptions);

        if (sortValidationFailure != null)
        {
            await RaiseEventAsync(sortValidationFailure);
            return GetApplicationsResult.Empty(pagingOptions);
        }

        Activity.Current?.AddEvent(new ActivityEvent("Getting applications from repository"));
        PagedList<ApplicationResult> pagedList = await _applicationRepository.GetApplicationsAsync(filterOptions, searchOptions, pagingOptions, sortOptions, cancellationToken);
        Activity.Current?.AddEvent(new ActivityEvent("Retrieved applications from repository"));

        GetApplicationsResult queryResult = new GetApplicationsResult(pagedList, pagedList.PaginationMetadata);

        return queryResult;
    }
}