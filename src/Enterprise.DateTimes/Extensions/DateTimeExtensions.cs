using Enterprise.DateTimes.Formatting;

namespace Enterprise.DateTimes.Extensions;

public static class DateTimeExtensions
{
    public static DateOnly DateOnly(this DateTime dateTime) => System.DateOnly.FromDateTime(dateTime);
    public static bool IsEarlierThan(this DateTime source, DateTime target) => source.CompareTo(target) < 0;
    public static bool IsEqualTo(this DateTime source, DateTime target) => source.CompareTo(target) == 0;
    public static bool IsLaterThan(this DateTime source, DateTime target) => source.CompareTo(target) > 0;
    public static string ToIso8601(this DateTime dateTime) => dateTime.ToString(DateTimeFormatStrings.Iso8601);
}