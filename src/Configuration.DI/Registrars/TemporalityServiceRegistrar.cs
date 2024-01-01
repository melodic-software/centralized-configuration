using Microsoft.Extensions.DependencyInjection;
using Enterprise.DateTimes.Current;
using Enterprise.DateTimes.Current.Abstract;

namespace Configuration.DI.Registrars;

internal class TemporalityServiceRegistrar
{
    internal static void RegisterTemporalityServices(IServiceCollection services)
    {
        services.AddSingleton(provider =>
        {
            ICurrentDateTimeService currentDateTimeService = new UtcDateTimeService();

            return currentDateTimeService;
        });
    }
}