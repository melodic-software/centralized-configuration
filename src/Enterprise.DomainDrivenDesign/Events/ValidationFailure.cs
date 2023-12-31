using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Enterprise.DomainDrivenDesign.Events;

public class ValidationFailure(string message, string context) : DomainEvent
{
    public string Message { get; } = message;

    /// <summary>
    /// A key used to identify context of validation.
    /// This is typically a property name.
    /// </summary>
    public string Context { get; } = context;
}