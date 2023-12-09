using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Configuration.API.EntityFramework.Interceptors;

public class CustomDbCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        // customizations can be made here

        return result;
    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        // customizations can be made here

        return result;
    }
}