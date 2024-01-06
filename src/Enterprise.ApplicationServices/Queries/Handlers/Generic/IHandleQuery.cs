using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Handlers.Generic;

public interface IHandleQuery<TResult>
{
    Task<TResult> HandleAsync(IQuery query, CancellationToken cancellationToken);
}

public interface IHandleQuery<in TQuery, TResult> : IApplicationService where TQuery : IQuery
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}

// TODO: do the same type of dynamic dependency registration
// get all IHandleQuery<TQuery, TResult> and register them as MediatR IRequestHandler<TQuery, Result<TResult>>