using Enterprise.ApplicationServices.Queries.Model;

namespace Configuration.ApplicationServices.Queries.Applications;

public class GetApplicationById : IQuery
{
    public Guid ApplicationId { get; }

    public GetApplicationById(Guid applicationId)
    {
        ApplicationId = applicationId;
    }
}