using AutoMapper;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.Routing;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers;

[Route(RouteTemplates.HttpConnectionInfo)]
[ApiController]
public class HttpConnectionInfoController : CustomControllerBase
{
    private readonly IMapper _mapper;

    public HttpConnectionInfoController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpGet(Name = RouteNames.GetHttpConnectionInfo)]
    [Produces(typeof(HttpConnectionInfoDto))]
    public ActionResult<HttpConnectionInfoDto> Get()
    {
        IHttpConnectionFeature? httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();

        if (httpConnectionFeature == null)
            return NotFound();

        HttpConnectionInfoDto result = _mapper.Map<HttpConnectionInfoDto>(httpConnectionFeature);

        return Ok(result);
    }
}