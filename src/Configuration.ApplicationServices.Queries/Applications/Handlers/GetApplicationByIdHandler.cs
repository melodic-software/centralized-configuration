using Configuration.Core.Queries.Model;
using Configuration.Core.Queries.Repositories;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers;

namespace Configuration.ApplicationServices.Queries.Applications.Handlers;

public sealed class GetApplicationByIdHandler(
    IApplicationServiceDependencies appServiceDependencies,
    IApplicationRepository applicationRepository)
    : QueryHandler<GetApplicationById, Application?>(appServiceDependencies)
{
    public override async Task<Application?> HandleAsync(GetApplicationById query, CancellationToken cancellationToken)
    {
        Application? application = await applicationRepository.GetByIdAsync(query.ApplicationId, cancellationToken);

        return application;
    }
}