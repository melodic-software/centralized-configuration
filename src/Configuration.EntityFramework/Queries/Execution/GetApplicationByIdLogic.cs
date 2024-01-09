using Configuration.ApplicationServices.Applications.GetApplicationById;
using Configuration.ApplicationServices.Applications.Shared;
using Configuration.EntityFramework.DbContexts.Configuration;
using Configuration.EntityFramework.Entities;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.Mapping.Properties.Services.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Configuration.EntityFramework.Queries.Execution
{
    public class GetApplicationByIdLogic : IQueryLogic<GetApplicationById, ApplicationResult?>
    {
        private readonly ConfigurationContext _context;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly ILogger<GetApplicationByIdLogic> _logger;

        public GetApplicationByIdLogic(ConfigurationContext context, IPropertyMappingService propertyMappingService, ILogger<GetApplicationByIdLogic> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _propertyMappingService = propertyMappingService;
            _logger = logger;
        }

        public async Task<ApplicationResult?> ExecuteAsync(GetApplicationById query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting application in repository with ID: {id}", query.ApplicationId);

            Stopwatch stopWatch = Stopwatch.StartNew();

            ApplicationEntity? entity = await _context.Applications.AsNoTracking()
                .Where(x => !x.IsDeleted)
                .FirstOrDefaultAsync(x => x.DomainId == query.ApplicationId, cancellationToken);

            stopWatch.Stop();

            _logger.LogDebug("Querying application with {id} finished in {milliseconds} milliseconds",
                query.ApplicationId, stopWatch.ElapsedMilliseconds);

            if (entity == null)
                return null;

            ApplicationResult application = Map(entity);

            return application;
        }

        private static ApplicationResult Map(ApplicationEntity entity)
        {
            return new ApplicationResult(
                entity.DomainId,
                entity.UniqueName,
                entity.Name,
                entity.AbbreviatedName,
                entity.Description,
                entity.IsActive
            );
        }
    }
}
