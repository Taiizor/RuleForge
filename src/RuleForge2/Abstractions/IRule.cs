using System.Threading.Tasks;
using RuleForge2.Core.Validation;

namespace RuleForge2.Abstractions
{
    /// <summary>
    /// Represents a validation rule for a property.
    /// </summary>
    /// <typeparam name="T">The type of the property to validate.</typeparam>
    public interface IRule<T>
    {
        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Validates the specified value.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A validation result.</returns>
        ValidationResult Validate(T value);

        /// <summary>
        /// Validates the specified value asynchronously.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        Task<ValidationResult> ValidateAsync(T value);
    }
}