using Configuration.API.Routing;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers;

[Route(RouteTemplates.ScheduledConfigurationChanges)]
[ApiController]
public class ScheduledConfigurationChanges : CustomControllerBase
{
    [HttpGet(Name = RouteNames.GetScheduledConfigurationChanges)]
    public IActionResult Get()
    {
        return Ok();
    }

    [HttpPost(Name = RouteNames.CreateScheduledConfigurationChange)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post()
    {
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpPut(Name = RouteNames.UpdateScheduledConfigurationChange)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Put()
    {
        // https://www.rfc-editor.org/rfc/rfc9110.html#name-put
        // https://www.rfc-editor.org/rfc/rfc9110.html#name-204-no-content
        return NoContent();
    }

    [HttpPatch(Name = RouteNames.PatchScheduledConfigurationChange)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Patch()
    {
        return NoContent();
    }

    [HttpDelete(Name = RouteNames.DeleteScheduledConfigurationChange)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Delete()
    {
        return NoContent();
    }
}