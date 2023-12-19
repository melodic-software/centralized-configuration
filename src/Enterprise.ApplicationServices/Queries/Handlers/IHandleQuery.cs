using Enterprise.ApplicationServices.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Handlers;

public interface IHandleQuery<TResult>
{
    Task<TResult> HandleAsync(IQuery query);
}