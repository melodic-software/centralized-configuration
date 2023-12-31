using Configuration.Domain.Applications;
using Microsoft.Extensions.Logging;

namespace Configuration.Dapper.Commands.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ILogger _logger;

        public ApplicationRepository(ILoggerFactory loggerFactory)
        {
            // you can specific logger categories by type (which includes namespace)
            _logger = loggerFactory.CreateLogger<ApplicationRepository>();

            // OR by using a custom named category
            //_logger = loggerFactory.CreateLogger("DataAccessLayer"); 
        }

        public Task<Application?> GetByIdAsync(Guid id)
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

        public Task DeleteApplicationAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
