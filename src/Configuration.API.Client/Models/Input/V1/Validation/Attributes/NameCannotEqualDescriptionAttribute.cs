using Configuration.API.Client.Models.Input.V1.Abstract;
using System.ComponentModel.DataAnnotations;
using static Configuration.API.Client.Models.Input.V1.Validation.Constants.ApplicationValidationMessages;

namespace Configuration.API.Client.Models.Input.V1.Validation.Attributes;

public class NameCannotEqualDescriptionAttribute : ValidationAttribute
{
    public NameCannotEqualDescriptionAttribute() :
        base(DescriptionMustBeDifferentThanName)
    {

    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            // this happened when executing an invalid PUT request without a body
            // in cases like this, the result sent back to the client should be a 415 "UnsupportedMediaType"
            // for now we're just going to handle this as "valid" and let the framework take over
            // TODO: figure out if we can set something to prevent this code from executing when we have a PUT, PATCH or POST with no body

            return ValidationResult.Success;
        }

        // NOTE: these validations are not ran if there are ANY data annotation validation failures
        // this is default behavior because cross property validation can involve more complex code
        // if this is an issue, consider writing all model validation using data annotations OR via IValidatableObject validation

        // one reason for using attributes (at the property level) is that they are executed BEFORE data annotations
        // data annotations still prevent processing of any class level validation :/

        // the alternate implementation of this can be done by implementing the IValidatableObject interface on the model

        // NOTE: this can be a collection of model objects with the attribute...
        object objectInstance = validationContext.ObjectInstance;

        if (value is not ApplicationManipulationDto model)
        {
            string errorMessage = $"Attribute {nameof(NameCannotEqualDescriptionAttribute)} " +
                                  $"must be applied to a {nameof(ApplicationManipulationDto)} or a derived type";

            throw new Exception(errorMessage);
        }

        if (model.Name == model.Description)
        {
            // this is a cross property validation rule
            // so we can specify all the properties involved
            string[] memberNames = { nameof(model.Name), nameof(model.Description) };

            // the main property
            //memberNames = new[] { nameof(Description) };

            // OR just reference the associated containing object / resource name
            //memberNames = new[] { "Application" };

            return new ValidationResult(DescriptionMustBeDifferentThanName, memberNames);
        }

        return ValidationResult.Success;
    }
}