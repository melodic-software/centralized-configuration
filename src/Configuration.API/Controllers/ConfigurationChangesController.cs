using Configuration.API.Routing.Constants;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers;

[Route(RouteTemplates.ConfigurationChanges)]
[ApiController]
public class ConfigurationChangesController : CustomControllerBase
{
    [HttpPost(Name = RouteNames.CreateConfigurationChange)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post()
    {
        return StatusCode(StatusCodes.Status201Created);
    }
}