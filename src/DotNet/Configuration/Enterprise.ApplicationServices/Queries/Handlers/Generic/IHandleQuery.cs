using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Handlers.Generic;

public interface IHandleQuery<in TQuery, TResult> : IApplicationService where TQuery : IQuery
{
    Task<TResult> HandleAsync(TQuery query);
}