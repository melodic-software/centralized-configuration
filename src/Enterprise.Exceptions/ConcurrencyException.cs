namespace Enterprise.Exceptions;

public sealed class ConcurrencyException : Exception
{
    public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}