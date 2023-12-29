namespace Configuration.API.Client.DTOs.Input.V1.Validation.Constants;

public static class ApplicationValidationMessages
{
    public const string DescriptionMaxLength = "The description cannot have more than 500 characters.";
    public const string DescriptionMustBeDifferentThanName = "The provided description should be different from the name.";
    public const string DescriptionRequired = "Please fill out a description.";
    public const string IsActiveRequired = "Please specify an active state for the application.";
    public const string NameRequired = "Please enter an application name.";
    public const string NameMaxLength = "The name cannot have more than 100 characters.";
}