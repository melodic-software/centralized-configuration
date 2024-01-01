namespace Enterprise.Calculations;

/// <summary>
/// Represents a generic calculation with a result and a predefined status.
/// </summary>
/// <typeparam name="TResult">The type of the result of the calculation.</typeparam>
public class Calculation<TResult>
{
    /// <summary>
    /// Gets the result of the calculation.
    /// </summary>
    public TResult? Result { get; }

    /// <summary>
    /// Gets the status of the calculation.
    /// </summary>
    public CalculationStatus Status { get; }

    /// <summary>
    /// A detailed reason that explains the status.
    /// </summary>
    protected string? Reason { get; }

    /// <summary>
    /// Gets the exception that occurred when the calculation was made (if applicable).
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// The date and time the calculation was made.
    /// </summary>
    public DateTimeOffset DateCompleted { get; }

    /// <summary>
    /// Indicates whether the calculation was successful.
    /// </summary>
    public bool WasSuccessful => Exception == null && Status == CalculationStatus.Successful;

    private Calculation(TResult? result, CalculationStatus status)
        : this(result, status, null, null, DateTimeOffset.UtcNow)
    {

    }

    private Calculation(TResult? result, CalculationStatus status, string? reason)
        : this(result, status, reason, null, DateTimeOffset.UtcNow)
    {

    }

    private Calculation(TResult? result, CalculationStatus status, Exception? exception) 
        : this(result, status, null, exception, DateTimeOffset.UtcNow)
    {

    }

    private Calculation(TResult? result, CalculationStatus status, string? reason, Exception? exception, DateTimeOffset dateCompleted)
    {
        Result = result;
        Status = status;
        Reason = reason;
        Exception = exception;
        DateCompleted = dateCompleted;
    }

    public static Calculation<TResult> Success(TResult result) => new(result, CalculationStatus.Successful);
    public static Calculation<TResult> CanNotBeMadeBecause(string reason) => new(default, CalculationStatus.Failed, reason);
    public static Calculation<TResult> FailedDueTo(Exception exception) => new(default, CalculationStatus.Failed, exception);

    public bool FailedBecause(string reason)
    {
        if (string.IsNullOrWhiteSpace(Reason) || string.IsNullOrWhiteSpace(reason))
            return false;

        return Reason == reason;
    }
}