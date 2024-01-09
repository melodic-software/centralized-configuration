using Configuration.ApplicationServices.Applications.Shared;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Enterprise.Events.Services.Raising;

namespace Configuration.ApplicationServices.Applications.GetApplicationByUniqueName;

public sealed class GetApplicationByUniqueNameHandler(
    IRaiseEvents eventRaiser,
    IEventCallbackService eventCallbackService,
    IApplicationRepository applicationRepository)
    : QueryHandler<GetApplicationByUniqueName, ApplicationResult?>(eventRaiser, eventCallbackService)
{
    public override async Task<ApplicationResult?> HandleAsync(GetApplicationByUniqueName query, CancellationToken cancellationToken)
    {
        ApplicationResult? application = await applicationRepository.GetByUniqueNameAsync(query.UniqueName, cancellationToken);

        return application;
    }
}