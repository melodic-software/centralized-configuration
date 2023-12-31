using Configuration.Core.Domain.Model.Entities;
using Configuration.Core.Domain.Services.Abstract;
using Enterprise.DomainDrivenDesign.Events;

namespace Configuration.Core.Domain.Services.Validation;

public class ApplicationValidationService
{
    public async Task<List<ValidationFailure>> ValidateNewAsync(Application application, IApplicationExistenceService applicationExistenceService)
    {
        List<ValidationFailure> validationFailures = new List<ValidationFailure>();

        bool applicationExists = await applicationExistenceService.ApplicationExistsAsync(application.Id);

        if (applicationExists)
        {
            ValidationFailure validationFailure = ApplicationValidationFailures.IdNotUnique(application.Id.ToString());
            validationFailures.Add(validationFailure);
        }

        validationFailures.AddRange(Validate(application));

        return validationFailures;
    }

    public List<ValidationFailure> Validate(Application application)
    {
        List<ValidationFailure> validationFailures = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(application.Name))
            validationFailures.Add(ApplicationValidationFailures.NameRequired);

        return validationFailures;
    }
}