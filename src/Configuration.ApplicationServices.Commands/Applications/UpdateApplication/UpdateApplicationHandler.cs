using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.DomainDrivenDesign.Events;

namespace Configuration.ApplicationServices.Commands.Applications.UpdateApplication;

public sealed class UpdateApplicationHandler(
    IApplicationServiceDependencies appServiceDependencies,
    IApplicationExistenceService applicationExistenceService,
    ApplicationValidationService applicationValidationService,
    IApplicationRepository applicationRepository)
    : CommandHandler<UpdateApplication>(appServiceDependencies)
{
    public override async Task HandleAsync(UpdateApplication command)
    {
        bool applicationExists = await applicationExistenceService.ApplicationExistsAsync(command.Id);

        if (!applicationExists)
        {
            ApplicationNotFound applicationNotFound = new ApplicationNotFound(command.Id);
            await RaiseEventAsync(applicationNotFound);
            return;
        }

        Application application = (await applicationRepository.GetByIdAsync(command.Id))!;

        application.Update(command.Name, command.AbbreviatedName, command.Description, command.IsActive);

        List<ValidationFailure> validationFailures = applicationValidationService.Validate(application);

        if (validationFailures.Any())
        {
            await RaiseEventsAsync(validationFailures);
            return;
        }

        await applicationRepository.Save(application);

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