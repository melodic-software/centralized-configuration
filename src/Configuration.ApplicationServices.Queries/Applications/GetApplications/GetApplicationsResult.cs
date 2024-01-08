using Configuration.ApplicationServices.Queries.Applications.Shared;
using Enterprise.Core.Queries.Paging;

namespace Configuration.ApplicationServices.Queries.Applications.GetApplications;

public class GetApplicationsResult
{
    public IEnumerable<ApplicationResult> Applications { get; }
    public PaginationMetadata PaginationMetadata { get; }

    public GetApplicationsResult(IEnumerable<ApplicationResult> applications, PaginationMetadata paginationMetadata)
    {
        Applications = applications;
        PaginationMetadata = paginationMetadata;
    }

    public static GetApplicationsResult Empty(PagingOptions pagingOptions)
    {
        List<ApplicationResult> items = new List<ApplicationResult>();
        PaginationMetadata paginationMetadata = new PaginationMetadata(0, pagingOptions);
        GetApplicationsResult result = new GetApplicationsResult(items, paginationMetadata);
        return result;
    }
}