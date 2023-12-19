namespace Enterprise.API.Exceptions;

public class ConfigurationException : Exception
{
    public ConfigurationException(string message) : base(message)
    {

    }

    public ConfigurationException(string message, Exception innerException) : base(message, innerException)
    {

    }
}