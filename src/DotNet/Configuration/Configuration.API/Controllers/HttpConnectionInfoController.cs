using AutoMapper;
using Configuration.API.Client.Models.Output.V1;
using Configuration.API.Routing.Constants;
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
    [Produces(typeof(HttpConnectionInfoModel))]
    public ActionResult<HttpConnectionInfoModel> Get()
    {
        IHttpConnectionFeature? httpConnectionFeature = HttpContext.Features.Get<IHttpConnectionFeature>();

        if (httpConnectionFeature == null)
            return NotFound();

        HttpConnectionInfoModel model = _mapper.Map<HttpConnectionInfoModel>(httpConnectionFeature);

        return Ok(model);
    }
}