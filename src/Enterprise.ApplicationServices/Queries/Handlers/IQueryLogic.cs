using Enterprise.ApplicationServices.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Handlers;

public interface IQueryLogic<in TQuery, TResult> where TQuery : IQuery
{
    Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken);
}