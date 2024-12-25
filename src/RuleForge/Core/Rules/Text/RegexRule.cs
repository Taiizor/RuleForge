using System.Text.RegularExpressions;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Text
{
    /// <summary>
    /// Rule that validates if a string matches a regular expression pattern.
    /// </summary>
    public class RegexRule : BaseRule<string>
    {
        private readonly string _propertyName;
        private readonly Regex _regex;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the RegexRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="pattern">The regular expression pattern.</param>
        /// <param name="options">Regular expression options.</param>
        public RegexRule(string propertyName, string pattern, RegexOptions options = RegexOptions.None)
        {
            _propertyName = propertyName;
            _regex = new Regex(pattern, options);
            ErrorMessage = $"{propertyName} is not in the correct format";
            ErrorMessageKey = "Regex";
        }

        /// <summary>
        /// Validates if the value matches the regular expression pattern.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            if (!_regex.IsMatch(value))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates if the value matches the regular expression pattern asynchronously.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(string value)
        {
            return Task.FromResult(Validate(value));
        }

        /// <summary>
        /// Validates if the value matches the regular expression pattern.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public override ValidationResult Validate(ValidationContext<string> context)
        {
            if (string.IsNullOrEmpty(context.Instance))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            if (!_regex.IsMatch(context.Instance))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }
    }
}