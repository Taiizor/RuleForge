using System.Text.RegularExpressions;

namespace RuleForge.Rules.Common
{
    public class PhoneNumberRule : IRule<string>
    {
        private static readonly Regex PhoneRegex = new(
            @"^\+?([0-9\s\-\(\)]{10,})$",
            RegexOptions.Compiled);

        public string ErrorMessage { get; }

        public PhoneNumberRule(string? errorMessage = null)
        {
            ErrorMessage = errorMessage ?? "Invalid phone number format";
        }

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            // Remove all non-digit characters for validation
            string digitsOnly = new(value.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length < 10 || digitsOnly.Length > 15 || !PhoneRegex.IsMatch(value))
            {
                return ValidationResult.Error("PhoneNumber", ErrorMessage);
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(string? value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}