using System.Linq;
using System.Text.RegularExpressions;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Text
{
    /// <summary>
    /// Rule that validates if a string is a valid credit card number.
    /// </summary>
    public class CreditCardRule : BaseRule<string>
    {
        private static readonly Regex _digitRegex = new Regex(@"[^\d]");
        private static readonly Regex CreditCardRegex = new(
            @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|6(?:011|5[0-9]{2})[0-9]{12}|(?:2131|1800|35\d{3})\d{11})$",
            RegexOptions.Compiled);

        private readonly string _propertyName;

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
            ErrorMessage = $"{propertyName} must be a valid credit card number";
            ErrorMessageKey = "CreditCard";
        }

        /// <summary>
        /// Validates if the value is a valid credit card number using the Luhn algorithm.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public override ValidationResult Validate(ValidationContext<string> context)
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

            var number = cardNumber.Replace(" ", "").Replace("-", "");

            if (!CreditCardRegex.IsMatch(number))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            // Luhn algorithm
            var sum = 0;
            var isEven = false;

            // Loop through values starting from the rightmost position
            for (var i = number.Length - 1; i >= 0; i--)
            {
                var digit = number[i] - '0';

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
        public override Task<ValidationResult> ValidateAsync(ValidationContext<string> context)
        {
            return Task.FromResult(Validate(context));
        }
    }
}