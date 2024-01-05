using Enterprise.ApplicationServices.Commands.Handlers.Generic;
using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.Validation;
using FluentValidation;
using ValidationException = Enterprise.Exceptions.ValidationException;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class FluentValidationDecorator<T>(
    IHandleCommand<T> commandHandler,
    IEnumerable<IValidator<T>> validators)
    : CommandHandlerDecorator<T>(commandHandler)
    where T : ICommand
{
    public override async Task HandleAsync(T command)
    {
        if (!validators.Any())
        {
            await DecoratedHandler.HandleAsync((dynamic)command);
            return;
        }

        IValidationContext context = new ValidationContext<T>(command);

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

        await DecoratedHandler.HandleAsync(command);
    }
}