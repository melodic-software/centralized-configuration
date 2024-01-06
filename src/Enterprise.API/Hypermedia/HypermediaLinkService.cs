using Enterprise.API.Client.Hypermedia;
using Enterprise.API.Client.Hypermedia.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.API.Hypermedia;

public class HypermediaLinkService
{
    public static HypermediaLinkDto CreateSelfLink(string? href)
    {
        string rel = Relations.Self;
        string method = HttpMethods.Get;
        HypermediaLinkDto linkModel = new HypermediaLinkDto(href, rel, method);
        return linkModel;
    }

    public static HypermediaLinkDto CreateSelfLink(ControllerBase controller, string routeName, object? values)
    {
        string rel = Relations.Self;
        string method = HttpMethods.Get;
        HypermediaLinkDto linkModel = CreateLink(controller, routeName, values, rel, method);
        return linkModel;
    }

    public static HypermediaLinkDto CreateLink(ControllerBase controller, string routeName, object? values, string rel, string method)
    {
        IUrlHelper urlHelper = controller.Url;
        string? href = urlHelper.Link(routeName, values);
        HypermediaLinkDto linkModel = new HypermediaLinkDto(href, rel, method);
        return linkModel;
    }
}