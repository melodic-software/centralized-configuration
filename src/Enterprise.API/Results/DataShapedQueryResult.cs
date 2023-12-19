using System.Dynamic;
using Enterprise.Core.Queries.Paging;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.API.Results;

public class DataShapedQueryResult
{
    public IActionResult? FailureActionResult { get; }
    public IEnumerable<ExpandoObject> DataShapedResult { get; }
    public PaginationMetadata PaginationMetadata { get; }

    private DataShapedQueryResult(IActionResult? failureActionResult, IEnumerable<ExpandoObject> dataShapedResult, PaginationMetadata paginationMetadata)
    {
        FailureActionResult = failureActionResult;
        DataShapedResult = dataShapedResult;
        PaginationMetadata = paginationMetadata;
    }

    public static DataShapedQueryResult Failure(IActionResult actionResult)
    {
        IEnumerable<ExpandoObject> dataShaped = new List<ExpandoObject>();
        PaginationMetadata paginationMetadata = PaginationMetadata.Empty();
        DataShapedQueryResult result = new DataShapedQueryResult(actionResult, dataShaped, paginationMetadata);
        return result;
    }

    public static DataShapedQueryResult Success(IEnumerable<ExpandoObject> dataShapedResult, PaginationMetadata paginationMetadata)
    {
        return new DataShapedQueryResult(null, dataShapedResult, paginationMetadata);
    }
}