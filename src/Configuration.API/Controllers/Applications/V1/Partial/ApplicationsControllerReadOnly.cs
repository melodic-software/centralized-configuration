using Configuration.API.Client.DTOs.Input.V1;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.Controllers.Applications.V1.Extensions;
using Configuration.API.Logging;
using Configuration.API.Routing;
using Configuration.ApplicationServices.Queries.Applications.GetApplicationById;
using Configuration.ApplicationServices.Queries.Applications.GetApplications;
using Configuration.ApplicationServices.Queries.Applications.GetByUniqueName;
using Enterprise.API.ActionConstraints;
using Enterprise.API.Client.Hypermedia;
using Enterprise.API.Constants;
using Enterprise.API.ContentNegotiation.Constants;
using Enterprise.API.Controllers.Extensions;
using Enterprise.API.Results;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Enterprise.Reflection.Properties.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Dynamic;
using Configuration.ApplicationServices.Queries.Applications.Shared;

namespace Configuration.API.Controllers.Applications.V1.Partial;

public partial class ApplicationsController
{
    /// <summary>
    /// Get applications.
    /// </summary>
    /// <param name="getApplicationsDto"></param>
    /// <param name="propertyExistenceService"></param>
    /// <param name="queryHandler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpHead(Name = RouteNames.HeadApplication)]
    [HttpGet(Name = RouteNames.GetApplications)]
    [RequestHeaderMatchesMediaType(HttpHeaderConstants.Accept, MediaTypeConstants.Json, MediaTypeConstants.Xml)] // This is a routing constraint.
    [Consumes(MediaTypeConstants.Json, MediaTypeConstants.Xml)]
    [Produces(MediaTypeConstants.Json, MediaTypeConstants.Xml, Type = typeof(List<ApplicationDto>))]
    public async Task<IActionResult> GetApplicationsWithoutLinks([FromQuery] GetApplicationsDto getApplicationsDto,
        [FromServices] IPropertyExistenceService propertyExistenceService,
        [FromServices] IHandleQuery<GetApplications, GetApplicationsResult> queryHandler,
        CancellationToken cancellationToken)
    {
        try
        {
            // TODO: this object isn't being represented (just a plain .ToString())
            using (_logger.BeginScope("ScopeModel: {ScopeModel}", getApplicationsDto))
            {
                DataShapedQueryResult result = await this
                    .GetApplications(getApplicationsDto, propertyExistenceService, queryHandler, _mapper, _logger, _problemDetailsFactory, cancellationToken);

                if (result.FailureActionResult != null)
                    return result.FailureActionResult;

                IEnumerable<ExpandoObject> resultModel = result.DataShapedResult;

                return Ok(resultModel);
            }
        }
        catch (Exception ex)
        {
            LogGettingApplicationsError(ex);
            throw;
        }
    }

    /// <summary>
    /// Get applications.
    /// </summary>
    /// <param name="getApplicationsDto"></param>
    /// <param name="propertyExistenceService"></param>
    /// <param name="queryHandler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpHead]
    [HttpGet(Name = RouteNames.GetApplications)]
    [RequestHeaderMatchesMediaType(HttpHeaderConstants.Accept, VendorMediaTypeConstants.HypermediaJson, VendorMediaTypeConstants.HypermediaXml)] // routing constraint
    [Consumes(VendorMediaTypeConstants.HypermediaJson, VendorMediaTypeConstants.HypermediaXml)]
    [Produces(VendorMediaTypeConstants.HypermediaJson, VendorMediaTypeConstants.HypermediaXml)] // TODO: add hypermedia model type?
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> GetApplicationsWithLinks([FromQuery] GetApplicationsDto getApplicationsDto,
        [FromServices] IPropertyExistenceService propertyExistenceService,
        [FromServices] IHandleQuery<GetApplications, GetApplicationsResult> queryHandler,
        CancellationToken cancellationToken)
    {
        try
        {
            DataShapedQueryResult result = await this
                .GetApplications(getApplicationsDto, propertyExistenceService, queryHandler, _mapper, _logger, _problemDetailsFactory, cancellationToken);

            if (result.FailureActionResult != null)
                return result.FailureActionResult;

            DataShapedHypermediaDto resultModel = this.ApplicationsWithLinks(getApplicationsDto, result.PaginationMetadata, result.DataShapedResult);

            return Ok(resultModel);
        }
        catch (Exception ex)
        {
            LogGettingApplicationsError(ex);
            throw;
        }
    }

    /// <summary>
    /// Get an application by ID.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /applications/500f86a2-65f7-4fc2-836a-2b14f8686209
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "applicationId": "500f86a2-65f7-4fc2-836a-2b14f8686209",
    ///         "uniqueName": "Demo Application-500f86a2",
    ///         "name": "Demo Application",
    ///         "abbreviatedName": "DEMO",
    ///         "description": null,
    ///         "isActive": true
    ///     }
    /// 
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="properties"></param>
    /// <param name="mediaType"></param>
    /// <param name="propertyExistenceService"></param>
    /// <param name="queryHandler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>an IActionResult</returns>
    /// <response code="200">Returns the requested application.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Application not found.</response>
    [HttpGet("{id:guid}", Name = RouteNames.GetApplicationById)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces(typeof(ApplicationDto))]
    public async Task<IActionResult> GetApplicationById(Guid id, string? properties,
        [FromHeader(Name = HttpHeaderConstants.Accept)] string? mediaType,
        [FromServices] IPropertyExistenceService propertyExistenceService,
        [FromServices] IHandleQuery<GetApplicationById, ApplicationResult?> queryHandler,
        CancellationToken cancellationToken)
    {
        LogGettingApplicationById(id);

        if (id == Guid.Empty)
            return BadRequest();

        // check if the inputted media type is a valid media type
        if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? parsedMediaType))
        {
            ProblemDetails problemDetails = _problemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: 400,
                detail: "Accept header media type value is not a valid media type."
            );

            return BadRequest(problemDetails);
        }

        if (!propertyExistenceService.TypeHasProperties<ApplicationDto>(properties))
            return this.BadDataShapingRequest(_problemDetailsFactory, properties);

        GetApplicationById query = new GetApplicationById(id);
        ApplicationResult? application = await queryHandler.HandleAsync(query, cancellationToken);

        if (application == null)
        {
            LogApplicationNotFound(id);
            return NotFound();
        }

        ExpandoObject resultModel = this.GetResultModel(application, properties, parsedMediaType, _mapper);

        return Ok(resultModel);
    }

    /// <summary>
    /// Get an application by unique name.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /applications/Demo Application-500f86a2
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "applicationId": "500f86a2-65f7-4fc2-836a-2b14f8686209",
    ///         "uniqueName": "Demo Application-500f86a2",
    ///         "name": "Demo Application",
    ///         "abbreviatedName": "DEMO",
    ///         "description": null,
    ///         "isActive": true
    ///     }
    /// 
    /// </remarks>
    /// <param name="uniqueName"></param>
    /// <param name="properties"></param>
    /// <param name="mediaType"></param>
    /// <param name="propertyExistenceService"></param>
    /// <param name="queryHandler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{uniqueName}", Name = RouteNames.GetApplicationByUniqueName)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetApplicationByUniqueName(string uniqueName, string? properties,
        [FromHeader(Name = HttpHeaderConstants.Accept)] string? mediaType,
        [FromServices] IPropertyExistenceService propertyExistenceService,
        [FromServices] IHandleQuery<GetApplicationByUniqueName, ApplicationResult?> queryHandler,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(uniqueName))
            return BadRequest();

        // check if the inputted media type is a valid media type
        if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? parsedMediaType))
        {
            ProblemDetails problemDetails = _problemDetailsFactory.CreateProblemDetails(
                HttpContext,
                statusCode: StatusCodes.Status400BadRequest,
                detail: "Accept header media type value is not a valid media type."
            );

            return BadRequest(problemDetails);
        }

        // if the client specified a data shape result, we need to ensure that the fields requested (if any) are valid
        if (!propertyExistenceService.TypeHasProperties<ApplicationDto>(properties))
            return this.BadDataShapingRequest(_problemDetailsFactory, properties);

        GetApplicationByUniqueName query = new GetApplicationByUniqueName(uniqueName);
        ApplicationResult? application = await queryHandler.HandleAsync(query, cancellationToken);

        if (application == null)
        {
            LogApplicationNotFound(uniqueName);
            return NotFound();
        }

        ExpandoObject resultModel = this.GetResultModel(application, properties, parsedMediaType, _mapper);

        return Ok(resultModel);
    }


    [LoggerMessage(ConfigApiEventIds.GettingApplicationById, LogLevel.Information, "Getting application with ID: {id}")]
    private partial void LogGettingApplicationById(Guid id);

    [LoggerMessage(ConfigApiEventIds.ApplicationNotFoundByUniqueName, LogLevel.Information, "Application with unique name \"{uniqueName}\" not found.")]
    private partial void LogApplicationNotFound(string uniqueName);

    [LoggerMessage(ConfigApiEventIds.ApplicationNotFoundById, LogLevel.Information, "Application with ID \"{id}\" not found.")]
    private partial void LogApplicationNotFound(Guid id);

    [LoggerMessage(ConfigApiEventIds.GettingApplicationsError, LogLevel.Critical, "Exception while getting applications.")]
    private partial void LogGettingApplicationsError(Exception exception);
}