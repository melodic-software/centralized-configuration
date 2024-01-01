using Configuration.Domain.Applications;
using Configuration.Domain.Applications.Events;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.DateTimes.Current.Abstract;
using Enterprise.DomainDrivenDesign.Event;
using Enterprise.MediatR.Adapters;

namespace Configuration.ApplicationServices.Applications.CreateApplication;

public class CreateApplicationHandler : CommandHandler<CreateApplication>
{
    private readonly ApplicationValidationService _applicationValidationService;
    private readonly IApplicationExistenceService _applicationExistenceService;
    private readonly IApplicationRepository _applicationRepository;
    private readonly ICurrentDateTimeService _currentDateTimeService;

    public CreateApplicationHandler(IApplicationServiceDependencies appServiceDependencies,
        ApplicationValidationService applicationValidationService,
        IApplicationExistenceService applicationExistenceService,
        IApplicationRepository applicationRepository,
        ICurrentDateTimeService currentDateTimeService) : base(appServiceDependencies)
    {
        _applicationValidationService = applicationValidationService;
        _applicationExistenceService = applicationExistenceService;
        _applicationRepository = applicationRepository;
        _currentDateTimeService = currentDateTimeService;
    }

    public override async Task HandleAsync(CreateApplication command)
    {
        DateTime currentDateTime = _currentDateTimeService.GetUtcNow();

        Application application = Application.New(command.Id, command.Name, command.AbbreviatedName, command.Description, command.IsActive);

        List<ValidationFailure> validationFailures = await _applicationValidationService.ValidateNewAsync(application, _applicationExistenceService);

        if (validationFailures.Any())
        {
            await RaiseEventsAsync(validationFailures);
        }
        else
        {
            await _applicationRepository.Save(application);

            await RaiseEventAsync(new ApplicationCreated(
                application.Id,
                application.UniqueName,
                application.Name,
                application.AbbreviatedName,
                application.Description,
                application.IsActive
            ));
        }
    }
}