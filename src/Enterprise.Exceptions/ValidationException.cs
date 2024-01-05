using Enterprise.Validation;

namespace Enterprise.Exceptions;

public class ValidationException(IEnumerable<ValidationError> validationErrors) : Exception
{
    public IEnumerable<ValidationError> ValidationErrors { get; } = validationErrors;
}