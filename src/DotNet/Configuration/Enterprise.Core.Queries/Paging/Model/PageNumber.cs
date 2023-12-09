using Enterprise.Core.Queries.Paging.Constants;

namespace Enterprise.Core.Queries.Paging.Model;

public class PageNumber
{
    public int Value { get; }

    public PageNumber(int? pageNumber, int? defaultPageNumber = null, int? minPageNumber = null)
    {
        defaultPageNumber ??= PagingConstants.DefaultPageNumber;
        minPageNumber ??= PagingConstants.MinPageNumber;

        pageNumber ??= defaultPageNumber;

        if (pageNumber < minPageNumber)
            pageNumber = defaultPageNumber;

        Value = pageNumber.Value;
    }

    public static PageNumber Default()
    {
        return new PageNumber(PagingConstants.DefaultPageNumber);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}