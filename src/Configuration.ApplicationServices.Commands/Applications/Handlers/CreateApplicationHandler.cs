using Configuration.Core.Domain.Model.Entities;
using Configuration.Core.Domain.Model.Events;
using Configuration.Core.Domain.Services.Abstract;
using Configuration.Core.Domain.Services.Abstract.Repositories;
using Configuration.Core.Domain.Services.Validation;
using Enterprise.ApplicationServices.Abstractions;
using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.DomainDrivenDesign.Events;

namespace Configuration.ApplicationServices.Commands.Applications.Handlers;

public class CreateApplicationHandler : CommandHandler<CreateApplication>
{
    private readonly ApplicationValidationService _applicationValidationService;
    private readonly IApplicationExistenceService _applicationExistenceService;
    private readonly IApplicationRepository _applicationRepository;

    public CreateApplicationHandler(IApplicationServiceDependencies applicationServiceDependencies,
        ApplicationValidationService applicationValidationService,
        IApplicationExistenceService applicationExistenceService,
        IApplicationRepository applicationRepository) : base(applicationServiceDependencies)
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