using Enterprise.ApplicationServices.Events;
using Enterprise.ApplicationServices.Queries.Model;
using Microsoft.Extensions.Logging;

namespace Enterprise.ApplicationServices.Queries.Handlers;

/// <summary>
/// Most query handler implementations end up being pretty thin...
/// Some pragmatic approaches involve writing the data access code directly in the handler, but this violates onion architecture.
/// One solution is to use a prebuilt handler implementation which requires a query logic implementation.
/// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public class SimpleQueryHandler<TQuery, TResult> : QueryHandlerBase<TQuery, TResult>
    where TQuery : IQuery
{
    private readonly IQueryLogic<TQuery, TResult> _queryLogic;

    /// <summary>
    /// Most query handler implementations end up being pretty thin and add a lot of overhead.
    /// A pragmatic approach to this would be to just write the data access code directly in the handler, but this violates onion architecture.
    /// One solution that retains onion architecture is to use a prebuilt handler implementation which requires an implementation of a query logic abstraction.
    /// We can move that out to an infrastructure layer, and simplify the creation and registration of query handlers.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="eventServiceFacade"></param>
    /// <param name="logger"></param>
    /// <param name="queryLogic"></param>
    public SimpleQueryHandler(IEventServiceFacade eventServiceFacade,
        ILogger<SimpleQueryHandler<TQuery, TResult>> logger,
        IQueryLogic<TQuery, TResult> queryLogic) : base(eventServiceFacade, logger)
    {
        _queryLogic = queryLogic;
    }

    public override async Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken)
    {
        TResult result = await _queryLogic.ExecuteAsync(query, cancellationToken);

        return result;
    }
}