using Enterprise.DomainDrivenDesign.Event;

namespace Configuration.Domain.Applications;

public static class ApplicationValidationFailures
{
    public static ValidationFailure IdNotUnique(string id) => new($"An application already exists with ID: {id}", nameof(Application.Id));
    public static ValidationFailure NameRequired = new($"{nameof(Application.Name)} is required", nameof(Application.Name));
}