using Configuration.Domain.Applications;
using Microsoft.Extensions.Logging;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;

namespace Configuration.Dapper.Commands.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly ILogger _logger;

    public ApplicationRepository(ILoggerFactory loggerFactory)
    {
        // You can specify logger categories by type (which includes namespace).
        _logger = loggerFactory.CreateLogger<ApplicationRepository>();

        // You can also specify by using a custom named category.
        //_logger = loggerFactory.CreateLogger("DataAccessLayer"); 
    }

    public Task<Application?> GetByIdAsync(ApplicationId id)
    {
        throw new NotImplementedException();
    }

    public Task<Application?> GetByUniqueNameAsync(string uniqueName)
    {
        throw new NotImplementedException();
    }

    public Task Save(Application application)
    {
        throw new NotImplementedException();
    }

    public Task DeleteApplicationAsync(ApplicationId id)
    {
        throw new NotImplementedException();
    }
}