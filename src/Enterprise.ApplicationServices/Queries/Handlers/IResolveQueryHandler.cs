using Enterprise.ApplicationServices.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Handlers;

public interface IResolveQueryHandler
{
    public IHandleQuery<TQuery, TResult> GetQueryHandler<TQuery, TResult>(TQuery query) where TQuery : IQuery;
}