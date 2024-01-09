using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Enterprise.DomainDrivenDesign.Events;

public class ValidationFailure : DomainEvent
{
    public ValidationFailure(string message, string context)
    {
        Message = message;
        Context = context;
    }

    public string Message { get; }

    /// <summary>
    /// A key used to identify context of validation.
    /// This is typically a property name.
    /// </summary>
    public string Context { get; }
}