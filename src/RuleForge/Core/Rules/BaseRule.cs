using System.Threading.Tasks;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules
{
    /// <summary>
    /// Base class for validation rules.
    /// </summary>
    /// <typeparam name="T">The type to validate.</typeparam>
    public abstract class BaseRule<T> : IRule<T>
    {
        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public virtual string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error message key for localization.
        /// </summary>
        public virtual string? ErrorMessageKey { get; set; }

        /// <summary>
        /// Gets or sets the message formatter.
        /// </summary>
        public virtual IMessageFormatter? MessageFormatter { get; set; }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public abstract ValidationResult Validate(ValidationContext<T> context);

        /// <summary>
        /// Validates the instance asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public virtual async Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            return await Task.FromResult(Validate(context));
        }
    }
}