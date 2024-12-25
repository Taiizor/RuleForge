using System.Threading.Tasks;
using RuleForge2.Core.Validation;

namespace RuleForge2.Abstractions
{
    /// <summary>
    /// Interface for validation rules.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    public interface IRule<T>
    {
        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error message key for localization.
        /// </summary>
        string? ErrorMessageKey { get; set; }

        /// <summary>
        /// Gets or sets the message formatter.
        /// </summary>
        IMessageFormatter? MessageFormatter { get; set; }

        /// <summary>
        /// Validates a value.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        ValidationResult Validate(ValidationContext<T> context);

        /// <summary>
        /// Validates a value asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        Task<ValidationResult> ValidateAsync(ValidationContext<T> context);
    }
}