using System.Text.RegularExpressions;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Text
{
    /// <summary>
    /// Rule that validates if a string is a valid email address.
    /// </summary>
    public class EmailRule : IRule<string>
    {
        private readonly string _propertyName;
        private static readonly Regex _emailRegex = new Regex(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error message key for localization.
        /// </summary>
        public string? ErrorMessageKey { get; set; }

        /// <summary>
        /// Gets or sets the message formatter.
        /// </summary>
        public IMessageFormatter? MessageFormatter { get; set; }

        /// <summary>
        /// Initializes a new instance of the EmailRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        public EmailRule(string propertyName)
        {
            _propertyName = propertyName;
            ErrorMessage = $"{propertyName} must be a valid email address";
            ErrorMessageKey = "Email";
        }

        /// <summary>
        /// Validates if the value is a valid email address.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<string> context)
        {
            if (string.IsNullOrEmpty(context.Instance))
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            if (!_emailRegex.IsMatch(context.Instance))
            {
                var errorMessage = MessageFormatter != null && !string.IsNullOrEmpty(ErrorMessageKey)
                    ? MessageFormatter.GetMessage(ErrorMessageKey, new Dictionary<string, object> 
                    { 
                        { "PropertyName", _propertyName } 
                    })
                    : ErrorMessage;

                return ValidationResult.Error(_propertyName, errorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates if the value is a valid email address asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(ValidationContext<string> context)
        {
            return Task.FromResult(Validate(context));
        }
    }
}
