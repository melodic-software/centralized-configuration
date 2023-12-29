using Configuration.API.Client.DTOs.Input.V1;
using Configuration.API.Routing.Constants;
using Enterprise.API.Model.Enums;
using Enterprise.Core.Queries.Paging;
using Microsoft.AspNetCore.Mvc;
using static Enterprise.API.Model.Enums.ResourceUriType;

namespace Configuration.API.Controllers.Applications.V1.Services;

public static class ApplicationsResourceUriService
{
    public static string? CreateResourceUri(ControllerBase controller, GetApplicationsDto dto, PaginationMetadata paginationMetadata, ResourceUriType resourceUriType)
    {
        // TODO: reduce assignment duplication

        switch (resourceUriType)
        {
            case PreviousPage:
            {
                if (!paginationMetadata.HasPreviousPage)
                    return null;

                return controller.Url.Link(RouteNames.GetApplications, new
                {
                    // TODO: can we add something that can camel case these property names?
                    name = dto.Name,
                    abbreviatedName = dto.AbbreviatedName,
                    isActive = dto.IsActive,
                    searchQuery = dto.SearchQuery,
                    orderBy = dto.OrderBy,
                    properties = dto.Properties,
                    pageNumber = paginationMetadata.CurrentPage.Value - 1, // TODO: look at adding operator on the class
                    pageSize = paginationMetadata.PageSize,
                });
            }
            case NextPage:
            {
                if (!paginationMetadata.HasNextPage)
                    return null;

                return controller.Url.Link(RouteNames.GetApplications, new
                {
                    // TODO: can we add something that can camel case these property names?
                    name = dto.Name,
                    abbreviatedName = dto.AbbreviatedName,
                    isActive = dto.IsActive,
                    searchQuery = dto.SearchQuery,
                    orderBy = dto.OrderBy,
                    properties = dto.Properties,
                    pageNumber = paginationMetadata.CurrentPage.Value + 1, // TODO: look at adding operator on the class
                    pageSize = paginationMetadata.PageSize,
                });
            }
            case Current:
            default:
                return controller.Url.Link(RouteNames.GetApplications, new
                {
                    // TODO: can we add something that can camel case these property names?
                    name = dto.Name,
                    abbreviatedName = dto.AbbreviatedName,
                    isActive = dto.IsActive,
                    searchQuery = dto.SearchQuery,
                    orderBy = dto.OrderBy,
                    properties = dto.Properties,
                    pageNumber = paginationMetadata.CurrentPage.Value,
                    pageSize = paginationMetadata.PageSize,
                });
        }
    }
}