using Configuration.Core.Queries.Model;
using Enterprise.Core.Queries.Paging;

namespace Configuration.ApplicationServices.Queries.Applications.Results;

public class GetApplicationsResult
{
    public IEnumerable<Application> Applications { get; }
    public PaginationMetadata PaginationMetadata { get; }

    public GetApplicationsResult(IEnumerable<Application> applications, PaginationMetadata paginationMetadata)
    {
        Applications = applications;
        PaginationMetadata = paginationMetadata;
    }

    public static GetApplicationsResult Empty(PagingOptions pagingOptions)
    {
        List<Application> items = new List<Application>();
        PaginationMetadata paginationMetadata = new PaginationMetadata(0, pagingOptions);
        GetApplicationsResult result = new GetApplicationsResult(items, paginationMetadata);
        return result;
    }
}