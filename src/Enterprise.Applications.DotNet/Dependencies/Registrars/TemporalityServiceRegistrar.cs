using Enterprise.DateTimes.Current;
using Enterprise.DateTimes.Current.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Applications.DotNet.Dependencies.Registrars;

internal class TemporalityServiceRegistrar
{
    internal static void RegisterTemporalityServices(IServiceCollection services)
    {
        services.AddTransient(provider =>
        {
            ICurrentDateTimeService currentDateTimeService = new UtcDateTimeService();

            return currentDateTimeService;
        });
    }
}