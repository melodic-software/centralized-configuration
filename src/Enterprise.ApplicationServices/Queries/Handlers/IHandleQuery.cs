﻿using Enterprise.ApplicationServices.Queries.Model;

namespace Enterprise.ApplicationServices.Queries.Handlers;

public interface IHandleQuery<TResult>
{
    Task<TResult> HandleAsync(IQuery query, CancellationToken cancellationToken);
}

public interface IHandleQuery<in TQuery, TResult> : IApplicationService where TQuery : IQuery
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}