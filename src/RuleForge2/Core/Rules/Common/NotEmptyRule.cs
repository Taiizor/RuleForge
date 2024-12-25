using System.Threading.Tasks;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Common
{
    /// <summary>
    /// Rule that validates that a value is not null or empty.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    public class NotEmptyRule<T> : IRule<T>
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
            ErrorMessage = $"{propertyName} cannot be empty";
        }

        /// <summary>
        /// Validates that the specified value is not null or empty.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(T value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates that the specified value is not null or empty asynchronously.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(T value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}