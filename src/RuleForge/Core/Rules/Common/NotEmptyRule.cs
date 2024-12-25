using System;
using System.Threading.Tasks;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Common
{
    /// <summary>
    /// Rule that validates that a value is not null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    public class NotEmptyRule<T> : BaseRule<T>
    {
        private readonly string _propertyName;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the NotEmptyRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        public NotEmptyRule(string propertyName)
        {
            _propertyName = propertyName;
            ErrorMessage = $"{propertyName} must not be empty";
            ErrorMessageKey = "NotEmpty";
        }

        /// <summary>
        /// Validates that the specified value is not null or empty.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            if (context.Instance is string str && string.IsNullOrWhiteSpace(str))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            if (context.Instance is System.Collections.IEnumerable collection && !collection.GetEnumerator().MoveNext())
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates that the specified value is not null or empty asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public override Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            return Task.FromResult(Validate(context));
        }
    }
}