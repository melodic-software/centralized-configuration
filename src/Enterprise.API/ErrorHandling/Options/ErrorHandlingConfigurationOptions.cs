using Enterprise.API.ErrorHandling.Constants;

namespace Enterprise.API.ErrorHandling.Options;

public class ErrorHandlingConfigurationOptions
{
    /// <summary>
    /// This is the friendly message that will be returned when internal server errors occur (500 status code).
    /// </summary>
    public string InternalServerErrorResponseDetailMessage { get; set; }

    public ErrorHandlingConfigurationOptions()
    {
        InternalServerErrorResponseDetailMessage = ErrorConstants.InternalServerErrorMessage;
    }
}