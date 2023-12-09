using Enterprise.API.Client.Hypermedia;
using Enterprise.API.Client.Hypermedia.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.API.Hypermedia;

public class HypermediaLinkService
{
    public static HypermediaLinkModel CreateSelfLink(string? href)
    {
        string rel = Relations.Self;
        string method = HttpMethods.Get;
        HypermediaLinkModel linkModel = new HypermediaLinkModel(href, rel, method);
        return linkModel;
    }

    public static HypermediaLinkModel CreateSelfLink(ControllerBase controller, string routeName, object? values)
    {
        string rel = Relations.Self;
        string method = HttpMethods.Get;
        HypermediaLinkModel linkModel = CreateLink(controller, routeName, values, rel, method);
        return linkModel;
    }

    public static HypermediaLinkModel CreateLink(ControllerBase controller, string routeName, object? values, string rel, string method)
    {
        IUrlHelper urlHelper = controller.Url;
        string? href = urlHelper.Link(routeName, values);
        HypermediaLinkModel linkModel = new HypermediaLinkModel(href, rel, method);
        return linkModel;
    }
}