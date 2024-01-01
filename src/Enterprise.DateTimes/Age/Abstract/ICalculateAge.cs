using Enterprise.Calculations;

namespace Enterprise.DateTimes.Age.Abstract;

/// <summary>
/// Provides functionality to calculate age.
/// </summary>
public interface ICalculateAge
{
    /// <summary>
    /// Calculates the age based on the given birth date.
    /// </summary>
    /// <param name="birthDate">The birth date to calculate age from.</param>
    /// <returns>A Calculation result indicating the age or the reason for failure.</returns>
    Calculation<int> CalculateAge(DateTimeOffset birthDate);
}