using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.ApplicationServices.Queries.Model;
using Enterprise.Domain.Generic;
using Enterprise.Domain.Validation;
using MediatR;

namespace Enterprise.MediatR.Adapters;

public class QueryHandlerAdapter<TQuery, TResult>(
    IApplicationServiceDependencies appServiceDependencies,
    IMediator mediator)
    : QueryHandler<TQuery, TResult>(appServiceDependencies),
        IRequestHandler<IRequest<Result<TResult>>, Result<TResult>>
    where TQuery : IQuery
{
    public override async Task<TResult> HandleAsync(TQuery query)
    {
        QueryAdapter<TQuery, TResult> adapter = new QueryAdapter<TQuery, TResult>(query);
        Result<TResult> result = await Handle(adapter, CancellationToken.None);
        return result.Value;
    }

    public async Task<Result<TResult>> Handle(IRequest<Result<TResult>> request, CancellationToken cancellationToken)
    {
        Result<TResult> result = await mediator.Send(request, cancellationToken);

        if (result.IsFailure)
        {
            Error error = result.Error;
        }

        if (result.IsSuccess)
        {

        }

        return result.Value;
    }
}