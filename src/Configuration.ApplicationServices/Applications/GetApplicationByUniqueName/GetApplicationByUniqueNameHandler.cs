using Configuration.ApplicationServices.Applications.Shared;
using Enterprise.ApplicationServices.Events;
using Enterprise.ApplicationServices.Queries.Handlers;
using Microsoft.Extensions.Logging;

namespace Configuration.ApplicationServices.Applications.GetApplicationByUniqueName;

public sealed class GetApplicationByUniqueNameHandler : QueryHandlerBase<GetApplicationByUniqueName, ApplicationResult?>
{
    private readonly IApplicationRepository _applicationRepository;

    public GetApplicationByUniqueNameHandler(IEventServiceFacade eventServiceFacade,
        ILogger<GetApplicationByUniqueNameHandler> logger,
        IApplicationRepository applicationRepository) : base(eventServiceFacade, logger)
    {
        _applicationRepository = applicationRepository;
    }

    public override async Task<ApplicationResult?> HandleAsync(GetApplicationByUniqueName query, CancellationToken cancellationToken)
    {
        ApplicationResult? application = await _applicationRepository.GetByUniqueNameAsync(query.UniqueName, cancellationToken);

        return application;
    }
}