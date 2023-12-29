namespace Enterprise.API.ErrorHandling.Model
{
    public class ErrorDetailsDto(int statusCode, string message)
    {
        public int StatusCode { get; } = statusCode;
        public string Message { get; } = message;
    }
}
