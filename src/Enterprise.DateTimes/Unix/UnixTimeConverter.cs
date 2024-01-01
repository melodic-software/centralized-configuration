using Enterprise.DateTimes.Unix.Abstract;

namespace Enterprise.DateTimes.Unix
{
    /// <summary>
    /// Provides methods for converting between Unix time and .NET's DateTime and DateTimeOffset.
    /// Unix time is the number of seconds that have elapsed since January 1, 1970 (midnight UTC/GMT),
    /// not counting leap seconds. The class ensures that all DateTime and DateTimeOffset objects
    /// are treated as UTC, maintaining consistency across conversions.
    /// </summary>
    public class UnixTimeConverter : IUnixTimeConverter
    {
        /// <summary>
        /// Converts Unix time in seconds to a DateTime object in UTC.
        /// </summary>
        /// <param name="unixSeconds">The Unix time in seconds.</param>
        /// <returns>A DateTime object representing the specified Unix time in UTC.</returns>
        public DateTime ConvertToDateTime(long unixSeconds)
        {
            return DateTime.UnixEpoch.AddSeconds(unixSeconds);
        }

        /// <summary>
        /// Converts a DateTime object to Unix time in seconds.
        /// Assumes the input DateTime is in UTC.
        /// </summary>
        /// <param name="dateTime">The DateTime object to convert.</param>
        /// <returns>The Unix time in seconds corresponding to the given DateTime.</returns>
        public long ConvertToUnixSeconds(DateTime dateTime)
        {
            DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
            dateTimeOffset = dateTimeOffset.ToUniversalTime();
            long unixSeconds = dateTimeOffset.ToUnixTimeSeconds();
            return unixSeconds;
        }

        /// <summary>
        /// Converts Unix time in seconds to a DateTimeOffset object in UTC.
        /// </summary>
        /// <param name="unixSeconds">The Unix time in seconds.</param>
        /// <returns>A DateTimeOffset object representing the specified Unix time in UTC.</returns>
        public DateTimeOffset ConvertToDateTimeOffset(long unixSeconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixSeconds);
        }

        /// <summary>
        /// Converts a DateTimeOffset object to Unix time in seconds.
        /// </summary>
        /// <param name="dateTime">The DateTimeOffset object to convert.</param>
        /// <returns>The Unix time in seconds corresponding to the given DateTimeOffset.</returns>
        public long ConvertToUnixSeconds(DateTimeOffset dateTime)
        {
            return dateTime.ToUnixTimeSeconds();
        }
    }
}
