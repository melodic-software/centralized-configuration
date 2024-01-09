using Configuration.ApplicationServices.Applications.Shared;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Microsoft.Extensions.Logging;

namespace Configuration.ApplicationServices.Applications.GetApplicationByUniqueName;

public sealed class GetApplicationByUniqueNameHandler : QueryHandler<GetApplicationByUniqueName, ApplicationResult?>
{
    private readonly IApplicationRepository _applicationRepository;

    public GetApplicationByUniqueNameHandler(IRaiseEvents eventRaiser,
        IEventCallbackService eventCallbackService,
        ILogger<GetApplicationByUniqueNameHandler> logger,
        IApplicationRepository applicationRepository) : base(eventRaiser, eventCallbackService, logger)
    {
        _applicationRepository = applicationRepository;
    }

    public override async Task<ApplicationResult?> HandleAsync(GetApplicationByUniqueName query, CancellationToken cancellationToken)
    {
        ApplicationResult? application = await _applicationRepository.GetByUniqueNameAsync(query.UniqueName, cancellationToken);

        return application;
    }
}