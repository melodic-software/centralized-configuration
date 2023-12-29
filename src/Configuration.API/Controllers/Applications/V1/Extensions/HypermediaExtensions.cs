using Configuration.API.Client.Models.Input.V1;
using Configuration.API.Client.Models.Output.V1;
using Configuration.API.Controllers.Applications.V1.Services;
using Configuration.API.Routing.Constants;
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
    public static dynamic ApplicationsWithLinks(this ControllerBase controller, GetApplicationsDto getApplicationsDto,
        PaginationMetadata paginationMetadata, IEnumerable<ExpandoObject> dataShapedResult)
    {
        // hypermedia links
        IEnumerable<HypermediaLinkModel> links = controller.CreateLinksForApplications(getApplicationsDto, paginationMetadata);

        // create links for each application in the list
        IEnumerable<IDictionary<string, object?>> value = dataShapedResult.Select(application =>
        {
            IDictionary<string, object?> dictionary = application;
            Guid applicationId = (Guid)(dictionary[nameof(ApplicationDto.Id)] ?? throw new InvalidOperationException());
            IEnumerable<HypermediaLinkModel> applicationLinks = controller.CreateLinksForApplication(applicationId, null);
            application.TryAdd(HypermediaConstants.RootPropertyName, applicationLinks);
            return dictionary;
        });

        // anonymous type
        // this uses an envelope... which breaks REST
        // TODO: come back and fix this
        var resultModel = new { value, links };

        return resultModel;
    }

    public static IEnumerable<HypermediaLinkModel> CreateLinksForApplication(this ControllerBase controller, Guid id, string? properties)
    {
        // https://app.pluralsight.com/course-player?clipId=3fde0dd9-02e7-4170-abef-d9e4f4f7694a

        List<HypermediaLinkModel> links = new()
        {
            string.IsNullOrWhiteSpace(properties)
                ? HypermediaLinkService.CreateSelfLink(controller, RouteNames.GetApplicationById, new { id })
                : HypermediaLinkService.CreateSelfLink(controller, RouteNames.GetApplicationById, new { id, properties })
        };

        // TODO: add other links depending on what actions can be taken with this resource
        // this can be driven by business rules...

        return links;
    }

    public static IEnumerable<HypermediaLinkModel> CreateLinksForApplications(this ControllerBase controller,
        GetApplicationsDto getApplicationsDto, PaginationMetadata paginationMetadata)
    {
        List<HypermediaLinkModel> links = new();

        // self
        string? currentHref = ApplicationsResourceUriService.CreateResourceUri(controller, getApplicationsDto, paginationMetadata, ResourceUriType.Current);
        links.Add(HypermediaLinkService.CreateSelfLink(currentHref));

        // pagination
        if (paginationMetadata.HasPreviousPage)
        {
            string? previousPageHref = ApplicationsResourceUriService.CreateResourceUri(controller, getApplicationsDto, paginationMetadata, ResourceUriType.PreviousPage);
            links.Add(new(previousPageHref, Relations.PreviousPage, HttpMethods.Get));
        }

        if (paginationMetadata.HasNextPage)
        {
            string? nextPageHref = ApplicationsResourceUriService.CreateResourceUri(controller, getApplicationsDto, paginationMetadata, ResourceUriType.NextPage);
            links.Add(new(nextPageHref, Relations.NextPage, HttpMethods.Get));
        }

        return links;
    }
}