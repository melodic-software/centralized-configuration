using Enterprise.ApplicationServices.Queries.Model;

namespace Configuration.ApplicationServices.Queries.Applications.GetApplications;

public class GetApplications : IQuery
{
    public string? Name { get; }
    public string? AbbreviatedName { get; }
    public string? SearchQuery { get; }
    public bool? IsActive { get; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? OrderBy { get; }

    public GetApplications(string? name, string? abbreviatedName, bool? isActive, string? searchQuery, int? pageNumber, int? pageSize, string? orderBy)
    {
        Name = name;
        AbbreviatedName = abbreviatedName;
        IsActive = isActive;
        SearchQuery = searchQuery;
        PageNumber = pageNumber;
        PageSize = pageSize;
        OrderBy = orderBy;
    }
}