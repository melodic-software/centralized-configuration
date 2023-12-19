namespace Enterprise.Core.Queries.Sorting;

public class SortOptions
{
    public string? OrderBy { get; set; }

    public SortOptions(string? orderBy)
    {
        OrderBy = orderBy;

        // TODO: split these into properties?
        // absorb some of the logic in the property mapping / dynamic sort process?
        // we might as well give as much logic to the core 'query" library
    }
}