namespace RuleForge.Rules
{
    /// <summary>
    /// Represents a validation rule that can be applied to a value.
    /// </summary>
    /// <typeparam name="T">The type of the value being validated</typeparam>
    public interface IRule<T>
    {
        /// <summary>
        /// Validates the specified value synchronously.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>A validation result indicating success or failure</returns>
        ValidationResult Validate(T value);

        /// <summary>
        /// Validates the specified value asynchronously.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>A task that represents the asynchronous validation operation</returns>
        Task<ValidationResult> ValidateAsync(T value);

        /// <summary>
        /// Gets the error message for this rule.
        /// </summary>
        string ErrorMessage { get; }
    }
}