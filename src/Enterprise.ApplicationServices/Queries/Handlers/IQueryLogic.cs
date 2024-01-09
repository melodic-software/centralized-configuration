using Enterprise.ApplicationServices.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Handlers;

/// <summary>
/// Implementations perform the query execution and mapping logic.
/// This is used to externalize the actual query implementation to an infrastructure layer.
/// This reduces the number of components that need to be created while retaining onion architecture.
/// When partnered with prebuilt base query handlers and DI factory methods, this becomes more streamlined.
/// </summary>
/// <typeparam name="TQuery"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IQueryLogic<in TQuery, TResult> where TQuery : IQuery
{
    /// <summary>
    /// Execute the query logic.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TResult> ExecuteAsync(TQuery query, CancellationToken cancellationToken);
}