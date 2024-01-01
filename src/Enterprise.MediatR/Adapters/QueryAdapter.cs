using Enterprise.ApplicationServices.Queries.Model;
using Enterprise.Domain.Generic;
using MediatR;

namespace Enterprise.MediatR.Adapters;

public class QueryAdapter<TQuery, TResult>(TQuery query) : IRequest<Result<TResult>> where TQuery : IQuery
{
    public TQuery Query { get; } = query;
}