namespace Enterprise.Domain.Validation;

public class Error
{
    public string Code { get; }
    public string Message { get; }

    public static Error None = new(string.Empty, string.Empty);
    public static Error NullValue = new("Error.NullValue", "Null value was provided.");

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}