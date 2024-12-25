using System.Text.RegularExpressions;

namespace RuleForge.Rules.Common
{
    public class RegexRule : IRule<string>
    {
        private readonly Regex _regex;
        public string ErrorMessage { get; }

        public RegexRule(string pattern, RegexOptions options = RegexOptions.None, string? errorMessage = null)
        {
            _regex = new(pattern, options);
            ErrorMessage = errorMessage ?? $"Value must match pattern: {pattern}";
        }

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            return _regex.IsMatch(value) 
                ? ValidationResult.Success() 
                : ValidationResult.Error("Pattern", ErrorMessage);
        }

        public Task<ValidationResult> ValidateAsync(string? value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}