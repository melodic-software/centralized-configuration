using Enterprise.DateTimes.Formatting;
namespace Enterprise.DateTimes.Extensions;

public static class DateTimeOffsetExtensions
{
    public static DateOnly DateOnly(this DateTimeOffset dateTimeOffset) => System.DateOnly.FromDateTime(dateTimeOffset.DateTime);
    public static bool IsEarlierThan(this DateTimeOffset source, DateTimeOffset target) => source.CompareTo(target) < 0;
    public static bool IsEqualTo(this DateTimeOffset source, DateTimeOffset target) => source.CompareTo(target) == 0;
    public static bool IsLaterThan(this DateTimeOffset source, DateTimeOffset target) => source.CompareTo(target) > 0;
    public static string ToIso8601(this DateTimeOffset dateTimeOffset) => dateTimeOffset.ToString(DateTimeFormatStrings.Iso8601);
}