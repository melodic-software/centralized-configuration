using Enterprise.DateTimes.Current.Abstract;
using Enterprise.NodaTime.Model;

namespace Enterprise.NodaTime.Current;

public class NodaTimeUtcDateTimeService : ICurrentDateTimeService
{
    public DateTime GetUtcNow() => new NodaUniversalDateTime().DateTime;

    public DateTimeOffset GetUtcNowOffset() => new NodaUniversalDateTime().DateTimeOffset;
}