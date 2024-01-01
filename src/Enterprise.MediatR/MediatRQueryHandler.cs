using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.ApplicationServices.Queries.Model;
using Enterprise.Domain.Generic;
using Enterprise.MediatR.Adapters;
using MediatR;

namespace Enterprise.MediatR;

public class MediatRQueryHandler<TQuery, TResult>(
    IApplicationServiceDependencies appServiceDependencies,
    IMediator mediator) : QueryHandler<TQuery, TResult>(appServiceDependencies) where TQuery : IQuery
{
    public override async Task<TResult> HandleAsync(TQuery query)
    {
        IRequest<Result<TResult>> adapter = new QueryAdapter<TQuery, TResult>(query);
        Result<TResult> result = await mediator.Send(adapter, CancellationToken.None);
        return result.Value;
    }
}