namespace Enterprise.Calculations
{
    public enum CalculationStatus
    {
        /// <summary>
        /// The calculation completed successfully.
        /// </summary>
        Successful,

        /// <summary>
        /// The calculation did not complete successfully.
        /// This is typically due to an exception.
        /// </summary>
        Failed
    }
}
