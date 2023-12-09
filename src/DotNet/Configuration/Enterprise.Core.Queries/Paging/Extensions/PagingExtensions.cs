using Enterprise.Core.Queries.Paging.Model;

namespace Enterprise.Core.Queries.Paging.Extensions;

public static class PagingExtensions
{
    public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagingOptions pagingOptions)
    {
        PageSize pageSize = pagingOptions.PageSize;
        PageNumber pageNumber = pagingOptions.PageNumber;

        int itemsToSkip = pageSize.Value * (pageNumber.Value - 1);
        int itemsToTake = pageSize.Value;

        IQueryable<T> pagedQuery = query.Skip(itemsToSkip).Take(itemsToTake);

        return pagedQuery;
    }

    public static async Task<PagedList<TResult>> ToPagedListAsync<TSource, TResult>(this IQueryable<TSource> query, PagingOptions pagingOptions, 
        Func<IQueryable<TSource>, Task<List<TSource>>> toListAsync, Func<TSource, TResult> map)
    {
        return await PagedList<TSource>.CreateAsync(query, pagingOptions, toListAsync, map);
    }
}