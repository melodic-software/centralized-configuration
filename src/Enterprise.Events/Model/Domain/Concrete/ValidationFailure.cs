using Enterprise.Events.Model.Domain.Abstract;

namespace Enterprise.Events.Model.Domain.Concrete;

public class ValidationFailure : DomainEvent
{
    public string Message { get; }

    /// <summary>
    /// A key used to identify context of validation.
    /// This is typically a property name.
    /// </summary>
    public string Context { get; }

    public ValidationFailure(string message, string context)
    {
        Message = message;
        Context = context;
    }
}