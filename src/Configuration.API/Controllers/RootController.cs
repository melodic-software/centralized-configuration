using Configuration.API.Routing.Constants;
using Enterprise.API.Client.Hypermedia;
using Enterprise.API.Client.Hypermedia.Constants;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;
using HttpMethods = Enterprise.API.Client.Hypermedia.Constants.HttpMethods;

namespace Configuration.API.Controllers;

[Route("")]
[ApiController]
public class RootController : CustomControllerBase
{
    private readonly IConfiguration _configuration;

    public RootController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet(Name = RouteNames.GetRoot)]
    public IActionResult GetRoot()
    {
        // here we generate links to the root document itself
        // and links to any actions that can happen on URIs at the root level OR that are not accessible otherwise

        // links to child resources should not be returned here
        // it is intended that the client application would access and traverse through the parent resource (which should be listed here)

        List<HypermediaLinkModel> links = new List<HypermediaLinkModel>
        {
            new(Url.Link(RouteNames.GetRoot, new { }), Relations.Self, HttpMethods.Get),
            new(Url.Link(RouteNames.GetApplications, new { }), "applications", HttpMethods.Get),
            new (Url.Link(RouteNames.CreateApplication, new {}), "create-application", HttpMethods.Post)
        };

        // other approaches and options for hypermedia:
        // https://app.pluralsight.com/course-player?clipId=6e7c0a9b-1003-416f-91a1-dc6d79894255

        // HAL (Hypertext Application Language)
        // https://datatracker.ietf.org/doc/html/draft-kelly-json-hal-08
        // this never left the draft stage and has expired since November 2016 

        // SIREN (Structured Interface for Representing Entities)
        // https://github.com/kevinswiber/siren

        // https://github.com/yury-sannikov/NHateoas
        // https://json-ld.org
        // https://jsonapi.org

        // https://www.odata.org (this goes WAY beyond HATEOAS)

        string? environment = _configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");

        return Ok(links);
    }
}