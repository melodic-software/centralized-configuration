using Configuration.Domain.Applications;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;

namespace Configuration.Dapper.Commands.Services;

public class ApplicationExistenceService : IApplicationExistenceService
{
    public Task<bool> ApplicationExistsAsync(ApplicationId id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ApplicationExistsAsync(string uniqueName)
    {
        throw new NotImplementedException();
    }
}