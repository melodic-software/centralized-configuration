using Enterprise.Core.Queries.Paging.Model;
using static Enterprise.Core.Queries.Paging.Constants.PagingConstants;

namespace Enterprise.Core.Queries.Paging;

public class PagingOptions
{
    public PageNumber PageNumber { get; }
    public PageSize PageSize { get; }

    public PagingOptions(int? pageNumber, int? pageSize)
        : this(pageNumber, pageSize, DefaultPageNumber, MinPageNumber, DefaultPageSize, MinPageSize, MaxPageSize)
    {

    }

    public PagingOptions(int? pageNumber, int? pageSize, int defaultPageSize, int maxPageSize)
        : this(pageNumber, pageSize, DefaultPageNumber, MinPageNumber, defaultPageSize, MinPageSize, maxPageSize)
    {

    }

    public PagingOptions(int? pageNumber, int? pageSize, int defaultPageNumber, int minPageNumber, int defaultPageSize, int minPageSize, int maxPageSize)
    {
        PageNumber = new PageNumber(pageNumber, defaultPageNumber, minPageNumber);
        PageSize = new PageSize(pageSize, defaultPageSize, minPageSize, maxPageSize);
    }

    public override string ToString()
    {
        return $"Page Number: {PageNumber} Page Size: {PageSize}";
    }
}