namespace Enterprise.API.Routing;

public static class RouteTemplates
{
    public const string ErrorHandlers = "error-handlers";
    public const string DevelopmentErrorHandler = $"/{ErrorHandlers}/{DevelopmentErrors}";
    public const string DevelopmentErrors = "development-errors";
    public const string ErrorHandler = $"/{ErrorHandlers}/{Errors}";
    public const string Errors = "errors";
}