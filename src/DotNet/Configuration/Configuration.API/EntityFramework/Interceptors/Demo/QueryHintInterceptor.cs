using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace Configuration.API.EntityFramework.Interceptors.Demo;

public class QueryHintInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        ApplyQueryHint(command);
        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
        CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        ApplyQueryHint(command);
        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private void ApplyQueryHint(DbCommand command)
    {
        if (!command.CommandText.ToUpper().Contains("EntityName"))
            return;

        // example: DBA recommends a query hint for any access to the table tied to the given entity
        command.CommandText += " OPTIONS (PLAN NAME)";
    }
}