namespace Enterprise.API.Client.Pagination;

public class PagingMetadataModel
{
    public int TotalCount { get; }
    public int PageSize { get; }
    public int CurrentPage { get; }
    public int TotalPages { get; }

    public PagingMetadataModel(int totalCount, int pageSize, int currentPage, int totalPages)
    {
        TotalCount = totalCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = totalPages;
    }
}