using Enterprise.Core.Queries.Paging.Constants;

namespace Enterprise.Core.Queries.Paging.Model;

public class PageSize
{
    public int Value { get; }

    public PageSize(int? pageSize, int? defaultPageSize = null, int? minPageSize = null, int? maxPageSize = null)
    {
        defaultPageSize ??= PagingConstants.DefaultPageSize;
        minPageSize ??= PagingConstants.MinPageSize;
        maxPageSize ??= PagingConstants.MaxPageSize;

        pageSize ??= defaultPageSize;

        if (pageSize < minPageSize)
            pageSize = defaultPageSize.Value;

        if (pageSize > maxPageSize)
            pageSize = maxPageSize.Value;

        Value = pageSize.Value;
    }

    public static PageSize Default()
    {
        return new PageSize(PagingConstants.DefaultPageSize);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}