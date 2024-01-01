using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Enterprise.ApplicationServices.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Handlers;

public abstract class QueryHandler<TQuery, TResult> : ApplicationService, 
    IHandleQuery<TResult>, IHandleQuery<TQuery, TResult> where TQuery : IQuery
{
    protected QueryHandler(IApplicationServiceDependencies appServiceDependencies)
        : base(appServiceDependencies)
    {

    }

    public async Task<TResult> HandleAsync(IQuery query)
    {
        Validate(query);

        // this is a dynamic dispatch
        TResult result = await HandleAsync((dynamic)query);

        return result;
    }

    public abstract Task<TResult> HandleAsync(TQuery query);

    private void Validate(IQuery query)
    {
        Type genericArgumentType = typeof(TQuery);
        Type queryType = query.GetType();

        bool queryCanBeHandled = queryType == genericArgumentType;

        if (queryCanBeHandled)
            return;

        Type queryHandlerType = GetType();

        throw new Exception(QueryCannotBeHandled(queryType, queryHandlerType));
    }

    private static string QueryCannotBeHandled(Type queryType, Type queryHandlerType) =>
        $"A query of type \"{queryType.FullName}\" cannot be handled by \"{queryHandlerType.FullName}\"";
}