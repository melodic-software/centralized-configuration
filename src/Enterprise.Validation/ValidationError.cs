namespace Enterprise.Validation;

public record ValidationError
{
    public ValidationError(string PropertyName, string ErrorMessage)
    {
        this.PropertyName = PropertyName;
        this.ErrorMessage = ErrorMessage;
    }

    public string PropertyName { get; init; }
    public string ErrorMessage { get; init; }

    public void Deconstruct(out string PropertyName, out string ErrorMessage)
    {
        PropertyName = this.PropertyName;
        ErrorMessage = this.ErrorMessage;
    }
}