using Configuration.ApplicationServices.Queries.Applications.Shared;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers;

namespace Configuration.ApplicationServices.Queries.Applications.GetByUniqueName;

public sealed class GetApplicationByUniqueNameHandler(
    IApplicationServiceDependencies appServiceDependencies,
    IApplicationRepository applicationRepository)
    : QueryHandler<GetApplicationByUniqueName, ApplicationResult?>(appServiceDependencies)
{
    public override async Task<ApplicationResult?> HandleAsync(GetApplicationByUniqueName query, CancellationToken cancellationToken)
    {
        ApplicationResult? application = await applicationRepository.GetByUniqueNameAsync(query.UniqueName, cancellationToken);

        return application;
    }
}