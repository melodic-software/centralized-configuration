using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Enterprise.Logging.Extensions;

public static class LoggingBuilderExtensions
{
    public static void RemoveConsoleLogger(this ILoggingBuilder logging)
    {
        Type providerType = typeof(Microsoft.Extensions.Logging.Console.ConsoleLoggerProvider);
        logging.RemoveProvider(providerType);
    }

    public static void RemoveProvider(this ILoggingBuilder logging, Type providerType)
    {
        foreach (ServiceDescriptor serviceDescriptor in logging.Services)
        {
            if (serviceDescriptor.ImplementationType != providerType)
                continue;
            
            logging.Services.Remove(serviceDescriptor);
            
            break;
        }
    }
}