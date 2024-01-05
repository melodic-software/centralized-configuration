using Enterprise.MediatR.Adapters.Abstract;
using Enterprise.Validation;
using FluentValidation;
using MediatR;
using ValidationException = Enterprise.Exceptions.ValidationException;

namespace Enterprise.MediatR.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    IValidator<TRequest> validator)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommandAdapter
{
    private readonly IValidator<TRequest> _validator = validator;

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next();

        IValidationContext context = new ValidationContext<TRequest>(request);

        List<ValidationError> validationErrors = validators
            .Select(validator => validator.Validate(context))
            .Where(validationResult => validationResult.Errors.Any())
            .SelectMany(validationResult => validationResult.Errors)
            .Select(validationFailure => new ValidationError(
                validationFailure.PropertyName,
                validationFailure.ErrorMessage))
            .ToList();

        if (validationErrors.Any())
        {
            // This should be handled in an upper layer - like a global error handling middleware.
            throw new ValidationException(validationErrors);
        }

        return await next();
    }
}