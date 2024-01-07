using Configuration.API.Client.DTOs.Input.V1;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.Controllers.Applications.V1.Services;
using Configuration.API.Routing;
using Enterprise.API.Client.Hypermedia;
using Enterprise.API.Client.Hypermedia.Constants;
using Enterprise.API.Hypermedia;
using Enterprise.API.Model.Enums;
using Enterprise.Core.Queries.Paging;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using HttpMethods = Enterprise.API.Client.Hypermedia.Constants.HttpMethods;

namespace Configuration.API.Controllers.Applications.V1.Extensions;

public static class HypermediaExtensions
{
    public static DataShapedHypermediaDto ApplicationsWithLinks(this ControllerBase controller, GetApplicationsDto getApplicationsDto,
        PaginationMetadata paginationMetadata, IEnumerable<ExpandoObject> dataShapedResult)
    {
        // hypermedia links
        List<HypermediaLinkDto> links = controller.CreateLinksForApplications(getApplicationsDto, paginationMetadata);

        // create links for each application in the list
        List<IDictionary<string, object?>> value = dataShapedResult.Select(application =>
        {
            IDictionary<string, object?> dictionary = application;
            Guid applicationId = (Guid)(dictionary[nameof(ApplicationDto.Id)] ?? throw new InvalidOperationException());
            IEnumerable<HypermediaLinkDto> applicationLinks = controller.CreateLinksForApplication(applicationId, null);
            application.TryAdd(HypermediaConstants.RootPropertyName, applicationLinks);
            return dictionary;
        }).ToList();

        DataShapedHypermediaDto resultModel = new DataShapedHypermediaDto
        {
            Value = value,
            Links = links
        };

        return resultModel;
    }

    public static List<HypermediaLinkDto> CreateLinksForApplication(this ControllerBase controller, Guid id, string? properties)
    {
        // https://app.pluralsight.com/course-player?clipId=3fde0dd9-02e7-4170-abef-d9e4f4f7694a

        List<HypermediaLinkDto> links =
        [
            string.IsNullOrWhiteSpace(properties)
                ? HypermediaLinkService.CreateSelfLink(controller, RouteNames.GetApplicationById, new { id })
                : HypermediaLinkService.CreateSelfLink(controller, RouteNames.GetApplicationById, new { id, properties })
        ];

        // TODO: add other links depending on what actions can be taken with this resource
        // this can be driven by business rules...

        return links;
    }

    public static List<HypermediaLinkDto> CreateLinksForApplications(this ControllerBase controller,
        GetApplicationsDto getApplicationsDto, PaginationMetadata paginationMetadata)
    {
        List<HypermediaLinkDto> links = new();

        // self
        string? currentHref = ApplicationsResourceUriService
            .CreateResourceUri(controller, getApplicationsDto, paginationMetadata, ResourceUriType.Current);
        
        links.Add(HypermediaLinkService.CreateSelfLink(currentHref));

        // pagination
        if (paginationMetadata.HasPreviousPage)
        {
            string? previousPageHref = ApplicationsResourceUriService.
                CreateResourceUri(controller, getApplicationsDto, paginationMetadata, ResourceUriType.PreviousPage);

            links.Add(new(previousPageHref, Relations.PreviousPage, HttpMethods.Get));
        }

        if (paginationMetadata.HasNextPage)
        {
            string? nextPageHref = ApplicationsResourceUriService
                .CreateResourceUri(controller, getApplicationsDto, paginationMetadata, ResourceUriType.NextPage);

            links.Add(new(nextPageHref, Relations.NextPage, HttpMethods.Get));
        }

        return links;
    }
}