using Enterprise.Core.Queries.Paging.Model;

namespace Enterprise.Core.Queries.Paging;

public class PaginationMetadata
{
    public int TotalCount { get; }
    public PageTotal TotalPages { get; }
    public PageSize PageSize { get; }
    public PageNumber CurrentPage { get; }
    public bool HasPreviousPage => CurrentPage.Value > 1;
    public bool HasNextPage => CurrentPage.Value < TotalPages.Value;

    public PaginationMetadata(int totalCount, PageSize pageSize, PageNumber currentPage)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = new PageTotal(totalCount, pageSize);
    }

    public PaginationMetadata(int totalCount, PagingOptions pagingOptions)
    {
        TotalCount = totalCount;
        PageSize = pagingOptions.PageSize;
        CurrentPage = pagingOptions.PageNumber;
        TotalPages = new PageTotal(totalCount, pagingOptions.PageSize);
    }

    public static PaginationMetadata Empty()
    {
        int totalCount = 0;
        PageSize pageSize = PageSize.Default();
        PageNumber pageNumber = PageNumber.Default();
        PaginationMetadata paginationMetadata = new PaginationMetadata(totalCount, pageSize, pageNumber);
        return paginationMetadata;
    }
}