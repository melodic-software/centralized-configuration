using Enterprise.ApplicationServices.Queries.Handlers;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Enterprise.ApplicationServices.Queries.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Applications.DotNetCore.Queries;

public class QueryHandlerResolver : IResolveQueryHandler
{
    private readonly IServiceProvider _serviceProvider;

    public QueryHandlerResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IHandleQuery<TQuery, TResult> GetQueryHandler<TQuery, TResult>(TQuery query) where TQuery : IQuery
    {
        IHandleQuery<TQuery, TResult> queryHandler = _serviceProvider.GetRequiredService<IHandleQuery<TQuery, TResult>>();

        return queryHandler;
    }
}