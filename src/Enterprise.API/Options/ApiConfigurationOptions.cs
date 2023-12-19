using Enterprise.API.Cors.Options;
using Enterprise.API.ErrorHandling.Options;
using Enterprise.API.Events;
using Enterprise.API.Security.Options;
using Enterprise.API.Swagger.Options;
using Enterprise.Logging.Options;
using Enterprise.Monitoring.Health.Options;

namespace Enterprise.API.Options;

public class ApiConfigurationOptions
{
    public CorsConfigurationOptions CorsConfigurationOptions { get; set; }
    public ErrorHandlingConfigurationOptions ErrorHandlingConfigurationOptions { get; set; }
    public ApiConfigurationEventHandlers EventHandlers { get; set; }
    public HealthCheckOptions HealthCheckOptions { get; set; }
    public HttpRequestMiddlewareOptions HttpRequestMiddlewareOptions { get; set; }
    public LoggingConfigurationOptions LoggingConfigurationOptions { get; set; }
    public ServiceConfigurationOptions ServiceConfigurationOptions { get; set; }
    public SwaggerConfigurationOptions SwaggerConfigurationOptions { get; set; }
        
    public ApiConfigurationOptions()
    {
        CorsConfigurationOptions = new CorsConfigurationOptions();
        ErrorHandlingConfigurationOptions = new ErrorHandlingConfigurationOptions();
        EventHandlers = new DefaultApiConfigurationEventHandlers();
        HealthCheckOptions = new HealthCheckOptions();
        HttpRequestMiddlewareOptions = new HttpRequestMiddlewareOptions();
        LoggingConfigurationOptions = new LoggingConfigurationOptions();
        ServiceConfigurationOptions = new ServiceConfigurationOptions();
        SwaggerConfigurationOptions = new SwaggerConfigurationOptions();
    }
}