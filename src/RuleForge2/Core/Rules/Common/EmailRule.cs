using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Common
{
    /// <summary>
    /// Rule that validates an email address.
    /// </summary>
    public class EmailRule : IRule<string>
    {
        private readonly string _propertyName;
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the EmailRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        public EmailRule(string propertyName)
        {
            _propertyName = propertyName;
            ErrorMessage = $"{propertyName} must be a valid email address";
        }

        /// <summary>
        /// Validates that the specified value is a valid email address.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(string value)
        {
            if (value == null)
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            if (!EmailRegex.IsMatch(value))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates that the specified value is a valid email address asynchronously.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(string value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}