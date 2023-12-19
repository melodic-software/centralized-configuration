using Enterprise.Core.Queries.Paging.Model;

namespace Enterprise.Core.Queries.Paging.Services;

public static class PagingCalculator
{
    public static int CalculateTotalPages(int totalCount, PageSize pageSize)
    {
        return CalculateTotalPages(totalCount, pageSize.Value);
    }

    public static int CalculateTotalPages(int totalCount, int pageSize)
    {
        int totalPages = 0;

        if (pageSize == 0)
            return totalPages;

        totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return totalPages;
    }
}