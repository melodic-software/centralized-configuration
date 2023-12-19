namespace Enterprise.Core.Queries.Searching;

public class SearchOptions
{
    public string? SearchQuery { get; }

    public SearchOptions(string? searchQuery)
    {
        SearchQuery = searchQuery?.Trim();
    }
}