using Configuration.ApplicationServices.Applications.GetApplicationById;
using Configuration.ApplicationServices.Applications.Shared;
using Dapper;
using Enterprise.ApplicationServices.Queries.Handlers;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace Configuration.Dapper.Queries.Execution
{
    public class GetApplicationByIdLogic(string? connectionString, ILogger<GetApplicationByIdLogic> logger) : IQueryLogic<GetApplicationById, ApplicationResult?>
    {
        private readonly IDbConnection _db = new SqlConnection(connectionString);
        private readonly ILogger<GetApplicationByIdLogic> _logger = logger;

        public async Task<ApplicationResult?> ExecuteAsync(GetApplicationById query, CancellationToken cancellationToken)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("ApplicationGuid", query.ApplicationId);

            CommandDefinition commandDefinition = new CommandDefinition(
                "dbo.usp_GetApplicationById",
                dynamicParameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
            );

            dynamic? result = await _db.QueryFirstOrDefaultAsync(commandDefinition);

            if (result == null)
                return null;

            Guid applicationId = result.ApplicationGuid;
            string uniqueName = result.UniqueName;
            string applicationName = result.ApplicationName;
            string abbreviatedName = result.AbbreviatedName;
            string? description = result.ApplicationDescription;
            bool isActive = result.IsActive;

            ApplicationResult application = new ApplicationResult(applicationId, uniqueName, applicationName, abbreviatedName, description, isActive);

            return application;
        }
    }
}
