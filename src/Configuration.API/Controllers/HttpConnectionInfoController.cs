using AutoMapper;
using Configuration.API.Client.Models.Output.V1;
using Configuration.API.Routing.Constants;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers;

[Route(RouteTemplates.HttpConnectionInfo)]
[ApiController]
public class HttpConnectionInfoController(IMapper mapper) : CustomControllerBase
{
    [HttpGet(Name = RouteNames.GetHttpConnectionInfo)]
    [Produces(typeof(HttpConnectionInfoDto))]
    public ActionResult<HttpConnectionInfoDto> Get()
    {
        IHttpConnectionFeature? httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();

        if (httpConnectionFeature == null)
            return NotFound();

        HttpConnectionInfoDto result = mapper.Map<HttpConnectionInfoDto>(httpConnectionFeature);

        return Ok(result);
    }
}