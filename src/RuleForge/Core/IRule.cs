using System;
using System.Threading.Tasks;
using RuleForge.Core.Validation;

namespace RuleForge.Core
{
    /// <summary>
    /// Interface for validation rules.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    public interface IRule<T>
    {
        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error message key for the rule.
        /// </summary>
        string ErrorMessageKey { get; set; }

        /// <summary>
        /// Gets or sets the message formatter for the rule.
        /// </summary>
        Func<string, object[], string> MessageFormatter { get; set; }

        /// <summary>
        /// Gets or sets the severity level for the rule.
        /// </summary>
        Severity Severity { get; set; }

        /// <summary>
        /// Validates the value.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        ValidationResult Validate(ValidationContext<T> context);

        /// <summary>
        /// Validates the value asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        Task<ValidationResult> ValidateAsync(ValidationContext<T> context);
    }
}