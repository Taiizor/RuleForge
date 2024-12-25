using System.Threading.Tasks;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Common
{
    /// <summary>
    /// Rule that validates the length of a string.
    /// </summary>
    public class LengthRule : BaseRule<string>
    {
        private readonly string _propertyName;
        private readonly int _minLength;
        private readonly int _maxLength;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the LengthRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="minLength">The minimum allowed length.</param>
        /// <param name="maxLength">The maximum allowed length.</param>
        public LengthRule(string propertyName, int minLength, int maxLength)
        {
            _propertyName = propertyName;
            _minLength = minLength;
            _maxLength = maxLength;
            ErrorMessage = $"{propertyName} must be between {minLength} and {maxLength} characters";
            ErrorMessageKey = "Length";
        }

        /// <summary>
        /// Validates that the specified value's length is within the allowed range.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(string value)
        {
            if (value == null)
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            if (value.Length < _minLength || value.Length > _maxLength)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates the length of the string.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public override ValidationResult Validate(ValidationContext<string> context)
        {
            if (string.IsNullOrEmpty(context.Instance))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            var length = context.Instance.Length;
            if (length < _minLength || length > _maxLength)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates that the specified value's length is within the allowed range asynchronously.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(string value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}