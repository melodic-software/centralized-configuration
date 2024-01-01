namespace Enterprise.DateTimes.Current.Abstract;

public interface ICurrentDateTimeService
{
    public DateTime GetUtcNow();
    public DateTimeOffset GetUtcNowOffset();
}