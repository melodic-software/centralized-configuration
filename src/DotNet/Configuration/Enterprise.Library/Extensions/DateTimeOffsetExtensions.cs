namespace Enterprise.Library.Extensions;

public static class DateTimeOffsetExtensions
{
    public static int GetCurrentAge(this DateTimeOffset dateTimeOffset, DateTimeOffset? dateOfDeath = null)
    {
        DateTime dateToCalculateTo = DateTime.UtcNow;

        if (dateOfDeath != null)
            dateToCalculateTo = dateOfDeath.Value.UtcDateTime;

        int age = dateToCalculateTo.Year - dateTimeOffset.Year;

        if (dateToCalculateTo < dateTimeOffset.AddYears(age))
            age--;

        return age;
    }
}