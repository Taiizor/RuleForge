using System.Linq;
using System.Text.RegularExpressions;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Text
{
    /// <summary>
    /// Rule that validates if a string is a valid credit card number.
    /// </summary>
    public class CreditCardRule : IRule<string>
    {
        private readonly string _propertyName;
        private static readonly Regex _digitRegex = new Regex(@"[^\d]");

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the CreditCardRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        public CreditCardRule(string propertyName)
        {
            _propertyName = propertyName;
            ErrorMessage = $"{propertyName} is not a valid credit card number";
        }

        /// <summary>
        /// Validates if the value is a valid credit card number using the Luhn algorithm.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<string> context)
        {
            if (string.IsNullOrEmpty(context.Instance))
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            // Remove non-digit characters
            var cardNumber = _digitRegex.Replace(context.Instance, "");

            // Check if the card number contains only digits and has a valid length
            if (!cardNumber.All(char.IsDigit) || cardNumber.Length < 13 || cardNumber.Length > 19)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            // Luhn algorithm
            var sum = 0;
            var isEven = false;

            // Loop through values starting from the rightmost position
            for (var i = cardNumber.Length - 1; i >= 0; i--)
            {
                var digit = cardNumber[i] - '0';

                if (isEven)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
                isEven = !isEven;
            }

            if (sum % 10 != 0)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates if the value is a valid credit card number asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(ValidationContext<string> context)
        {
            return Task.FromResult(Validate(context));
        }
    }
}