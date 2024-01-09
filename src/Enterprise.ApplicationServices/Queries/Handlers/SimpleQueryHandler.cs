using Enterprise.ApplicationServices.Queries.Model;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;

namespace Enterprise.ApplicationServices.Queries.Handlers;

/// <summary>
/// Most query handler implementations end up being pretty thin...
/// Some pragmatic approaches involve writing the data access code directly in the handler, but this violates onion architecture.
/// One solution is to use a prebuilt handler implementation which requires a query logic implementation.
/// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class SimpleQueryHandler<TQuery, TResult> : QueryHandler<TQuery, TResult>
    where TQuery : IQuery
{
    private readonly IQueryLogic<TQuery, TResult> _queryLogic;

    /// <summary>
    /// Most query handler implementations end up being pretty thin...
    /// Some pragmatic approaches involve writing the data access code directly in the handler, but this violates onion architecture.
    /// One solution is to use a prebuilt handler implementation which requires a query logic implementation.
    /// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="eventRaiser"></param>
    /// <param name="eventCallbackService"></param>
    /// <param name="queryLogic"></param>
    public SimpleQueryHandler(IRaiseEvents eventRaiser,
        IEventCallbackService eventCallbackService,
        IQueryLogic<TQuery, TResult> queryLogic) : base(eventRaiser, eventCallbackService)
    {
        _queryLogic = queryLogic;
    }

    public override async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        TResult result = await _queryLogic.ExecuteAsync(query, cancellationToken);

        return result;
    }

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