using Enterprise.Validation;

namespace Enterprise.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationError> validationErrors)
    {
        ValidationErrors = validationErrors;
    }

    public IEnumerable<ValidationError> ValidationErrors { get; }
}