using Enterprise.Domain.Generic;
using MediatR;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Enterprise.ApplicationServices.Queries.Model;

namespace Enterprise.MediatR.Adapters;

public class QueryHandlerAdapter<TQuery, TResult>(IHandleQuery<TQuery, TResult> queryHandler)
    : IRequestHandler<QueryAdapter<TQuery, TResult>, Result<TResult>> where TQuery : IQuery
{
    public async Task<Result<TResult>> Handle(QueryAdapter<TQuery, TResult> request, CancellationToken cancellationToken)
    {
        TResult queryResult = await queryHandler.HandleAsync(request.Query);

        return queryResult;
    }
}