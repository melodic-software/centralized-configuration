using System.Text.Json;
using Enterprise.API.Client.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.API.Pagination;

public static class PaginationResponseHeaderService
{
    public static void AddToResponseHeader(PagingMetadataModel pagingMetadataModel, ControllerBase controller)
    {
        string responseHeaderName = PaginationConstants.CustomPaginationHeader;

        // TODO: do we need to respect the "Accept" header on the request here or is this always going to be a JSON representation?
        // TODO: do we need to do anything about escaped unicode characters (for example, the "&" sign in the query string is escaped as \u0026)
        JsonSerializerDefaults serializationDefaults = JsonSerializerDefaults.Web;
        JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions(serializationDefaults);
        string paginationJson = JsonSerializer.Serialize(pagingMetadataModel, jsonSerializerOptions);

        controller.Response.Headers.Add(responseHeaderName, paginationJson);
    }
}