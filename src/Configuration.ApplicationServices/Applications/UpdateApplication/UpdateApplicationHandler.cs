using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.DomainDrivenDesign.Events;
using Enterprise.Events.Services.Raising;
using Enterprise.Events.Services.Raising.Callbacks.Facade.Abstractions;
using Microsoft.Extensions.Logging;
using ApplicationId = Configuration.Domain.Applications.ApplicationId;

namespace Configuration.ApplicationServices.Applications.UpdateApplication;

public sealed class UpdateApplicationHandler : CommandHandler<UpdateApplication>
{
    private readonly IApplicationExistenceService _applicationExistenceService;
    private readonly ApplicationValidationService _applicationValidationService;
    private readonly IApplicationRepository _applicationRepository;

    public UpdateApplicationHandler(IRaiseEvents eventRaiser,
        IEventCallbackService eventCallbackService,
        ILogger<CommandHandler<UpdateApplication>> logger,
        IApplicationExistenceService applicationExistenceService,
        ApplicationValidationService applicationValidationService,
        IApplicationRepository applicationRepository) : base(eventRaiser, eventCallbackService, logger)
    {
        _applicationExistenceService = applicationExistenceService;
        _applicationValidationService = applicationValidationService;
        _applicationRepository = applicationRepository;
    }

    public override async Task HandleAsync(UpdateApplication command)
    {
        ApplicationId applicationId = new ApplicationId(command.Id);

        bool applicationExists = await _applicationExistenceService.ApplicationExistsAsync(applicationId);

        if (!applicationExists)
        {
            // TODO: throw ApplicationNotFoundException?
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(applicationId.Value);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        Application? application = await _applicationRepository.GetByIdAsync(applicationId);

        if (application == null)
        {
            // TODO: throw ApplicationNotFoundException?
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(applicationId.Value);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        application.Update(command.Name, command.AbbreviatedName, command.Description, command.IsActive);

        List<ValidationFailure> validationFailures = _applicationValidationService.Validate(application);

        if (validationFailures.Any())
        {
            await RaiseEventsAsync(validationFailures);
            return;
        }

        await _applicationRepository.Save(application);

        ApplicationUpdated applicationUpdated = new ApplicationUpdated(
            application.Id.Value,
            application.UniqueName,
            application.Name,
            application.AbbreviatedName,
            application.IsActive
        );

        await RaiseEventAsync(applicationUpdated);
    }
}