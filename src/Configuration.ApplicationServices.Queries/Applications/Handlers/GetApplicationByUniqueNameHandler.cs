using Configuration.Core.Queries.Model;
using Configuration.Core.Queries.Repositories;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers;

namespace Configuration.ApplicationServices.Queries.Applications.Handlers;

public sealed class GetApplicationByUniqueNameHandler(
    IApplicationServiceDependencies appServiceDependencies,
    IApplicationRepository applicationRepository)
    : QueryHandler<GetApplicationByUniqueName, Application?>(appServiceDependencies)
{
    public override async Task<Application?> HandleAsync(GetApplicationByUniqueName query)
    {
        Application? application = await applicationRepository.GetByUniqueNameAsync(query.UniqueName);

        return application;
    }
}