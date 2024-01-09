using Enterprise.ApplicationServices.Commands.Handlers;
using Enterprise.ApplicationServices.Commands.Model;
using Enterprise.Validation;
using FluentValidation;
using ValidationException = Enterprise.Exceptions.ValidationException;

namespace Enterprise.ApplicationServices.Decorators.CommandHandlers;

public class FluentValidationDecorator<T> : CommandHandlerDecorator<T>
    where T : ICommand
{
    private readonly IEnumerable<IValidator<T>> _validators;

    public FluentValidationDecorator(IHandleCommand<T> commandHandler,
        IEnumerable<IValidator<T>> validators) : base(commandHandler)
    {
        _validators = validators;
    }

    public override async Task HandleAsync(T command)
    {
        if (!_validators.Any())
        {
            await DecoratedHandler.HandleAsync((dynamic)command);
            return;
        }

        IValidationContext context = new ValidationContext<T>(command);

        List<ValidationError> validationErrors = _validators
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
            // TODO: Do we want to use an exception here or raise validation failure events and return out.
            // The second option keeps in alignment with existing practices.
            throw new ValidationException(validationErrors);
        }

        await DecoratedHandler.HandleAsync(command);
    }
}