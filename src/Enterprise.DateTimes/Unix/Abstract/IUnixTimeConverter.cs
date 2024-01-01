namespace Enterprise.DateTimes.Unix.Abstract;

public interface IUnixTimeConverter
{
    /// <summary>
    /// Converts Unix time in seconds to a DateTime object.
    /// </summary>
    /// <param name="unixSeconds">The Unix time in seconds.</param>
    /// <returns>A DateTime object representing the specified Unix time.</returns>
    public DateTime ConvertToDateTime(long unixSeconds);

    /// <summary>
    /// Converts a DateTime object to Unix time in seconds.
    /// </summary>
    /// <param name="dateTime">The DateTime object to convert.</param>
    /// <returns>The Unix time in seconds corresponding to the given DateTime.</returns>
    public long ConvertToUnixSeconds(DateTime dateTime);

    /// <summary>
    /// Converts Unix time in seconds to a DateTimeOffset object.
    /// </summary>
    /// <param name="unixSeconds">The Unix time in seconds.</param>
    /// <returns>A DateTimeOffset object representing the specified Unix time.</returns>
    public DateTimeOffset ConvertToDateTimeOffset(long unixSeconds);

    /// <summary>
    /// Converts a DateTimeOffset object to Unix time in seconds.
    /// </summary>
    /// <param name="dateTime">The DateTimeOffset object to convert.</param>
    /// <returns>The Unix time in seconds corresponding to the given DateTimeOffset.</returns>
    public long ConvertToUnixSeconds(DateTimeOffset dateTime);
}