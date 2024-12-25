using System.Text.RegularExpressions;

namespace RuleForge.Rules.Common
{
    public class EmailRule : IRule<string>
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string ErrorMessage { get; }

        public EmailRule(string errorMessage = null)
        {
            ErrorMessage = errorMessage ?? "Invalid email address";
        }

        public ValidationResult Validate(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            if (!EmailRegex.IsMatch(value))
            {
                return ValidationResult.Error("Email", ErrorMessage);
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(string value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}