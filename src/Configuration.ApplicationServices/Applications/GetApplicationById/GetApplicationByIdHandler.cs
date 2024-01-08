using Configuration.ApplicationServices.Applications.Shared;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers;

namespace Configuration.ApplicationServices.Applications.GetApplicationById;

public sealed class GetApplicationByIdHandler(
    IApplicationServiceDependencies appServiceDependencies,
    IApplicationRepository applicationRepository)
    : QueryHandler<GetApplicationById, ApplicationResult?>(appServiceDependencies)
{
    public override async Task<ApplicationResult?> HandleAsync(GetApplicationById query, CancellationToken cancellationToken)
    {
        ApplicationResult? application = await applicationRepository.GetByIdAsync(query.ApplicationId, cancellationToken);

        return application;
    }
}