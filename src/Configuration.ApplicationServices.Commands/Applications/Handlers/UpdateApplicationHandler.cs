using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.DomainDrivenDesign.Event;

namespace Configuration.ApplicationServices.Commands.Applications.Handlers;

public class UpdateApplicationHandler : CommandHandler<UpdateApplication>
{
    private readonly IApplicationExistenceService _applicationExistenceService;
    private readonly ApplicationValidationService _applicationValidationService;
    private readonly IApplicationRepository _applicationRepository;

    public UpdateApplicationHandler(IApplicationServiceDependencies applicationServiceDependencies,
        IApplicationExistenceService applicationExistenceService,
        ApplicationValidationService applicationValidationService,
        IApplicationRepository applicationRepository) : base(applicationServiceDependencies)
    {
        _applicationExistenceService = applicationExistenceService;
        _applicationValidationService = applicationValidationService;
        _applicationRepository = applicationRepository;
    }

    public override async Task HandleAsync(UpdateApplication command)
    {
        bool applicationExists = await _applicationExistenceService.ApplicationExistsAsync(command.Id);

        if (!applicationExists)
        {
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(command.Id);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        Application application = (await _applicationRepository.GetByIdAsync(command.Id))!;

        application.Update(command.Name, command.AbbreviatedName, command.Description, command.IsActive);

        List<ValidationFailure> validationFailures = _applicationValidationService.Validate(application);

        if (validationFailures.Any())
        {
            await RaiseEventsAsync(validationFailures);
            return;
        }

        await _applicationRepository.Save(application);

        ApplicationUpdated applicationUpdated = new ApplicationUpdated(
            application.Id,
            application.UniqueName,
            application.Name,
            application.AbbreviatedName,
            application.IsActive
        );

        await RaiseEventAsync(applicationUpdated);
    }
}