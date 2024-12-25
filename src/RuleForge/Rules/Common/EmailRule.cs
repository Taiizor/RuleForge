using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RuleForge.Rules.Common
{
    public class EmailRule<T> : IRule<T>
    {
        private readonly string _propertyName;
        private readonly Regex _emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public string ErrorMessage { get; set; }

        public EmailRule(string propertyName)
        {
            _propertyName = propertyName;
            ErrorMessage = $"{propertyName} must be a valid email address";
        }

        public ValidationResult Validate(T value)
        {
            if (value == null || !_emailRegex.IsMatch(value.ToString()))
            {
                return new ValidationResult(false, ErrorMessage);
            }

            return new ValidationResult(true);
        }

        public Task<ValidationResult> ValidateAsync(T value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}