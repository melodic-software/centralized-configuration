using Enterprise.ApplicationServices.Events;
using Enterprise.ApplicationServices.Queries.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Queries.Handlers;

public abstract class QueryHandler<TQuery, TResult> : ApplicationService, IHandleQuery<TResult>,
    IHandleQuery<TQuery, TResult>
    where TQuery : IQuery
{
    protected QueryHandler(IEventServiceFacade eventServiceFacade,
        ILogger<QueryHandler<TQuery, TResult>> logger) : base(eventServiceFacade, logger)
    {
    }

    public async Task<TResult> HandleAsync(IQuery query, CancellationToken cancellationToken)
    {
        Validate(query);

        // This is a dynamic dispatch.
        TResult result = await HandleAsync((dynamic)query, cancellationToken);

        return result;
    }

    public abstract Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);

    private void Validate(IQuery query)
    {
        Type genericArgumentType = typeof(TQuery);
        Type queryType = query.GetType();

        bool queryCanBeHandled = queryType == genericArgumentType;

        if (queryCanBeHandled)
            return;

        Type queryHandlerType = GetType();

        throw new Exception(QueryCannotBeHandled(queryType, queryHandlerType));
    }

    private static string QueryCannotBeHandled(Type queryType, Type queryHandlerType) =>
        $"A query of type \"{queryType.FullName}\" cannot be handled by \"{queryHandlerType.FullName}\"";
}