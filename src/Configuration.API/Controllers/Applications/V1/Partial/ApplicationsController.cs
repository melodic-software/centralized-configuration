using AutoMapper;
using Configuration.API.Client.DTOs.Input.V1;
using Configuration.API.Client.DTOs.Output.V1;
using Configuration.API.Controllers.Applications.V1.Extensions;
using Configuration.API.Logging.Constants;
using Configuration.API.Routing.Constants;
using Configuration.ApplicationServices.Applications.CreateApplication;
using Configuration.ApplicationServices.Commands.Applications;
using Configuration.ApplicationServices.Queries.Applications;
using Configuration.Core.Queries.Model;
using Configuration.Domain.Applications.Events;
using Enterprise.API.Constants;
using Enterprise.API.Controllers.Abstract;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Queries.Handlers.Generic;
using Enterprise.DomainDrivenDesign.Events;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Configuration.API.Controllers.Applications.V1.Partial;

[Route(RouteTemplates.Applications)]
[ApiController]
public partial class ApplicationsController : CustomControllerBase
{
    private readonly ILogger<ApplicationsController> _logger;
    private readonly IMapper _mapper;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    public ApplicationsController(ILogger<ApplicationsController> logger, IMapper mapper, ProblemDetailsFactory problemDetailsFactory)
    {
        // in most cases this will never happen...
        // but it can if the default container is replaced with one that doesn't raise exceptions when an implementation cannot be located
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _problemDetailsFactory = problemDetailsFactory ?? throw new ArgumentNullException(nameof(problemDetailsFactory));

        // this is an alternative to constructor injection, but is considered an anti-pattern (see "Service Locator")
        //HttpContext.RequestServices.GetService<ILogger<ApplicationsController>>();
    }

    /// <summary>
    /// Update an application.
    /// If an application is not found, an application resource will be created.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="updateApplicationDto"></param>
    /// <param name="updateApplicationHandler"></param>
    /// <param name="createApplicationHandler"></param>
    /// <returns></returns>
    /// <response code="404">Application not found.</response>
    /// <response code="422">Validation failure.</response>
    /// <response code="204">Application updated.</response>
    /// <response code="201">Application created.</response>
    [HttpPut("{id:guid}", Name = RouteNames.UpdateApplication)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApplicationDto))]
    [Consumes(MediaTypeConstants.Json, MediaTypeConstants.Xml)]
    public async Task<IActionResult> Put(Guid id, UpdateApplicationDto updateApplicationDto,
        [FromServices] IHandleCommand<UpdateApplication> updateApplicationHandler,
        [FromServices] IHandleCommand<CreateApplication> createApplicationHandler)
    {
        // this is for full updates
        // all resource fields are either overwritten or set to their default values
        // we support UPSERT behavior here...

        return await this.Upsert(id, updateApplicationDto, updateApplicationHandler, createApplicationHandler, _mapper);
    }

    /// <summary>
    /// Partially update an application.
    /// If an application is not found, an application resource will be created.
    /// </summary>
    /// <param name="id">the ID of the application</param>
    /// <param name="patchDocument">The patch document describing the changes to be made</param>
    /// <param name="queryHandler"></param>
    /// <param name="updateApplicationHandler"></param>
    /// <param name="createApplicationHandler"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     [
    ///         {
    ///             "op": "replace",
    ///             "path": "/abbreviatedName",
    ///             "value": "DEMO"
    ///         }
    ///     ]
    /// 
    /// </remarks>
    [HttpPatch("{id:guid}", Name = RouteNames.PatchApplication)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApplicationDto))]
    [Consumes(MediaTypeConstants.JsonPatch)]
    public async Task<IActionResult> Patch(Guid id, JsonPatchDocument<UpdateApplicationDto>? patchDocument,
        [FromServices] IHandleQuery<GetApplicationById, Application?> queryHandler,
        [FromServices] IHandleCommand<UpdateApplication> updateApplicationHandler,
        [FromServices] IHandleCommand<CreateApplication> createApplicationHandler
        , CancellationToken cancellationToken)
    {
        // https://datatracker.ietf.org/doc/html/rfc6902
        // https://datatracker.ietf.org/doc/html/rfc6902#section-4 (operations)

        // add operations really on make sense where the resource is dynamic - like in a CRM
        // remove operations should remove the current value and set it to the default value for that property

        if (patchDocument == null)
            return BadRequest();

        // the "Microsoft.AspNetCore.Mvc.NewtonsoftJson" adds a transitive dependency for "Microsoft.AspNetCore.JsonPatch"

        GetApplicationById query = new GetApplicationById(id);
        Application? application = await queryHandler.HandleAsync(query, cancellationToken);

        UpdateApplicationDto? updateDto = application == null ?
            new UpdateApplicationDto() :
            _mapper.Map<UpdateApplicationDto>(application);

        // apply the patch document to the model we use for updates (PUT)
        // if there are any patch document application errors, they will be added to the model state
        patchDocument.ApplyTo(updateDto, ModelState);

        // this checks validation (using data annotations, validation attributes, etc.)
        // AFTER the patch has been applied
        if (!TryValidateModel(updateDto))
            return ValidationProblem(ModelState);

        // we support UPSERT behavior here...

        if (application != null)
            return await this.Update(id, updateDto, updateApplicationHandler);

        CreateApplicationDto createDto = _mapper.Map<CreateApplicationDto>(updateDto);
        createDto.Id = id;

        return await this.Create(createDto, createApplicationHandler, _mapper);
    }

    /// <summary>
    /// Create an application.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /applications
    ///     {
    ///         "id": "500f86a2-65f7-4fc2-836a-2b14f8686209",
    ///         "name": "Demo Application",
    ///         "abbreviation": null
    ///     }
    /// 
    /// Sample response:
    /// 
    ///     {
    ///         "applicationId": "500f86a2-65f7-4fc2-836a-2b14f8686209",
    ///         "uniqueName": "Demo Application-500f86a2",
    ///         "name": "Demo Application",
    ///         "abbreviatedName": null,
    ///         "description": null,
    ///         "isActive": false
    ///     }
    /// 
    /// </remarks>
    /// <param name="createDto"></param>
    /// <param name="commandHandler"></param>
    /// <returns></returns>
    [HttpPost(Name = RouteNames.CreateApplication)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApplicationDto))]
    [Consumes(MediaTypeConstants.Json, MediaTypeConstants.Xml)]
    [Produces(MediaTypeConstants.Json, MediaTypeConstants.Xml)]
    public async Task<IActionResult> Post(CreateApplicationDto? createDto,
        [FromServices] IHandleCommand<CreateApplication> commandHandler)
    {
        // Normally the ApiController attribute would catch this and automatically return a 400 "bad request".

        // If the default validation handling is disabled (SuppressModelStateInvalidFilter = false)
        // OR nullable annotations are enabled in the project AND the model type has been deemed nullable (adding ? to variable declaration)
        // THEN this will also pass through and require manual validation.

        if (createDto == null)
            return BadRequest(); // This is a client error, not a 422 (unprocessable entity).

        // See comments above on validation handling. Under specific (default) circumstances, we don't have to check model state.
        // however we've set "SuppressModelStateInvalidFilter" to false so a 422 to can be returned instead of the default 400 (which is less accurate)
        // normally these would be returned as ValidationProblem()
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        return await this.Create(createDto, commandHandler, _mapper);
    }

    /// <summary>
    /// Delete an application.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="commandHandler"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}", Name = RouteNames.DeleteApplication)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(Guid id, [FromServices] IHandleCommand<DeleteApplication> commandHandler)
    {
        DeleteApplication command = new DeleteApplication(id);

        List<ValidationFailure> validationFailures = new List<ValidationFailure>();
        bool applicationNotFound = false;
        bool applicationDeleted = false;

        commandHandler.RegisterEventCallback(new Action<ValidationFailure>(validationFailures.Add));
        commandHandler.RegisterEventCallback(new Action<ApplicationNotFound>(_ => applicationNotFound = true));
        commandHandler.RegisterEventCallback(new Action<ApplicationDeleted>(_ => applicationDeleted = true));
        await commandHandler.HandleAsync(command);

        if (applicationNotFound)
            return NotFound();

        if (validationFailures.Any())
        {
            validationFailures.ForEach(x => ModelState.AddModelError(x.Context, x.Message));
            return UnprocessableEntity(ModelState);
        }

        if (applicationDeleted)
            LogApplicationDeleted(id);

        return NoContent();
    }

    /// <summary>
    /// Retrieve the communication options available for the resource.
    /// </summary>
    /// <returns></returns>
    [HttpOptions(Name = RouteNames.OptionsApplication)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Options()
    {
        Response.Headers.Append("Allow", "GET,HEAD,PUT,PATCH,POST,DELETE,OPTIONS");
        // the Content-Length header is set to zero by the framework, no need to do it manually
        return Ok();
    }

    [LoggerMessage(ConfigurationApiEventIds.ApplicationDeleted, LogLevel.Information, "Application with ID \"{id}\" removed.")]
    private partial void LogApplicationDeleted(Guid id);
}