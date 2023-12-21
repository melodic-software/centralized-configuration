using Enterprise.Events.Model.Domain.Abstract;

namespace Enterprise.Events.Model.Domain.Concrete;

public class ValidationFailure(string message, string context) : DomainEvent
{
    public string Message { get; } = message;

    /// <summary>
    /// A key used to identify context of validation.
    /// This is typically a property name.
    /// </summary>
    public string Context { get; } = context;
}