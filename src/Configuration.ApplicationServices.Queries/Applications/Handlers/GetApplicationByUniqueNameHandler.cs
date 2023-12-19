using Configuration.Core.Queries.Model;
using Configuration.Core.Queries.Repositories;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers;

namespace Configuration.ApplicationServices.Queries.Applications.Handlers;

public class GetApplicationByUniqueNameHandler : QueryHandler<GetApplicationByUniqueName, Application?>
{
    private readonly IApplicationRepository _applicationRepository;

    public GetApplicationByUniqueNameHandler(IApplicationServiceDependencies applicationServiceDependencies,
        IApplicationRepository applicationRepository) : base(applicationServiceDependencies)
    {
        _applicationRepository = applicationRepository;
    }

    public override async Task<Application?> HandleAsync(GetApplicationByUniqueName query)
    {
        Application? application = await _applicationRepository.GetByUniqueNameAsync(query.UniqueName);

        return application;
    }
}