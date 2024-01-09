using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Events;
using Enterprise.DomainDrivenDesign.Events;
using Enterprise.Exceptions;

namespace Configuration.ApplicationServices.Applications.CreateApplication;

public sealed class CreateApplicationHandler : CommandHandlerBase<CreateApplication>
{
    private readonly ApplicationValidationService _applicationValidationService;
    private readonly IApplicationExistenceService _applicationExistenceService;
    private readonly IApplicationRepository _applicationRepository;

    public CreateApplicationHandler(IEventServiceFacade eventServiceFacade,
        ApplicationValidationService applicationValidationService,
        IApplicationExistenceService applicationExistenceService,
        IApplicationRepository applicationRepository) : base(eventServiceFacade)
    {
        _applicationValidationService = applicationValidationService;
        _applicationExistenceService = applicationExistenceService;
        _applicationRepository = applicationRepository;
    }

    public override async Task HandleAsync(CreateApplication command)
    {
        Application application = Application.New(command.Id, command.Name, command.AbbreviatedName, command.Description, command.IsActive);

        List<ValidationFailure> validationFailures = await _applicationValidationService.ValidateNewAsync(application, _applicationExistenceService);

        if (validationFailures.Any())
        {
            await RaiseEventsAsync(validationFailures);
        }
        else
        {
            try
            {
                // TODO: Replace with "Add" method.
                await _applicationRepository.Save(application);

                // TODO: Use IUnitOfWork to persist the changes.

            }
            catch (ConcurrencyException)
            {
                // TODO: Is this even needed here?
                // Only concurrency issue would be if the application with the same ID was created at the same time this was created.
                await RaiseEventAsync(new ValidationFailure($"The application with ID \"{application.Id}\" already exists.", nameof(application.Id)));
                return;
            }

            ApplicationCreated applicationCreated = new ApplicationCreated(
                application.Id.Value,
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