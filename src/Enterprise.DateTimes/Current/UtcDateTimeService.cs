using Enterprise.DateTimes.Current.Abstract;
using Enterprise.DateTimes.Model;

namespace Enterprise.DateTimes.Current;

public class UtcDateTimeService : ICurrentDateTimeService
{
    public DateTime GetUtcNow() => new UniversalDateTime().DateTime;

    public DateTimeOffset GetUtcNowOffset() => new UniversalDateTime().DateTimeOffset;
}