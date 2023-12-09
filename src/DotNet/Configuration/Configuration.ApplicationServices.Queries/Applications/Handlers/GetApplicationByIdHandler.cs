using Configuration.Core.Queries.Model;
using Configuration.Core.Queries.Repositories;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers;

namespace Configuration.ApplicationServices.Queries.Applications.Handlers;

public class GetApplicationByIdHandler : QueryHandler<GetApplicationById, Application?>
{
    private readonly IApplicationRepository _applicationRepository;

    public GetApplicationByIdHandler(IApplicationServiceDependencies applicationServiceDependencies, IApplicationRepository applicationRepository)
        : base(applicationServiceDependencies)
    {
        _applicationRepository = applicationRepository;
    }

    public override async Task<Application?> HandleAsync(GetApplicationById query)
    {
        Application? application = await _applicationRepository.GetByIdAsync(query.ApplicationId);

        return application;
    }
}