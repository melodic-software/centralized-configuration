using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.DateTimes.Current.Abstract;
using Enterprise.DomainDrivenDesign.Events;

namespace Configuration.ApplicationServices.Applications.CreateApplication;

public sealed class CreateApplicationHandler(
    IApplicationServiceDependencies appServiceDependencies,
    ApplicationValidationService applicationValidationService,
    IApplicationExistenceService applicationExistenceService,
    IApplicationRepository applicationRepository,
    ICurrentDateTimeService currentDateTimeService)
    : CommandHandler<CreateApplication>(appServiceDependencies)
{
    public override async Task HandleAsync(CreateApplication command)
    {
        DateTime currentDateTime = currentDateTimeService.GetUtcNow();

        Application application = Application.New(command.Id, command.Name, command.AbbreviatedName, command.Description, command.IsActive);

        List<ValidationFailure> validationFailures = await applicationValidationService.ValidateNewAsync(application, applicationExistenceService);

        if (validationFailures.Any())
        {
            await RaiseEventsAsync(validationFailures);
        }
        else
        {
            await applicationRepository.Save(application);

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