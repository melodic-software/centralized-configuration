using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.DomainDrivenDesign.Events;
using Enterprise.Exceptions;

namespace Configuration.ApplicationServices.Applications.CreateApplication;

public sealed class CreateApplicationHandler(
    IApplicationServiceDependencies appServiceDependencies,
    ApplicationValidationService applicationValidationService,
    IApplicationExistenceService applicationExistenceService,
    IApplicationRepository applicationRepository)
    : CommandHandler<CreateApplication>(appServiceDependencies)
{
    public override async Task HandleAsync(CreateApplication command)
    {
        Application application = Application.New(command.Id, command.Name, command.AbbreviatedName, command.Description, command.IsActive);

        List<ValidationFailure> validationFailures = await applicationValidationService.ValidateNewAsync(application, applicationExistenceService);

        if (validationFailures.Any())
        {
            await RaiseEventsAsync(validationFailures);
        }
        else
        {
            try
            {
                // TODO: Replace with "Add" method.
                await applicationRepository.Save(application);

                // TODO: Use IUnitOfWork to persist the changes.

            }
            catch (ConcurrencyException)
            {
                // TODO: Is this even needed here?
                // Only concurrency issue would be if the application with the same ID was created at the same time this was created.
                await RaiseEventAsync(new ValidationFailure("This application already exists.", nameof(application.Id)));
                return;
            }

            ApplicationCreated applicationCreated = new ApplicationCreated(
                application.Id,
                application.UniqueName,
                application.Name,
                application.AbbreviatedName,
                application.Description,
                application.IsActive
            );

            await RaiseEventAsync(applicationCreated);
        }
    }
}