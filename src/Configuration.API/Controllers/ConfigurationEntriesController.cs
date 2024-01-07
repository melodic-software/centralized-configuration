using Configuration.API.Client.DTOs.Input.V1;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.InMemory;
using Configuration.API.Routing;
using Enterprise.API.Controllers.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers;

[Route(RouteTemplates.ConfigurationEntries)]
[ApiController]
public class ConfigurationEntriesController : CustomControllerBase
{
    [HttpGet(Name = RouteNames.GetConfigurationEntries)]
    [Produces(typeof(List<ConfigurationEntryDto>))]
    public IActionResult Get([FromQuery] GetConfigurationEntriesDto queryParams)
    {
        List<ConfigurationEntryDto> result = ConfigurationDataStore.ConfigurationEntries;

        return Ok(result);
    }

    [HttpPost(Name = RouteNames.CreateConfigurationEntry)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post()
    {
        return StatusCode(StatusCodes.Status201Created);
    }
}