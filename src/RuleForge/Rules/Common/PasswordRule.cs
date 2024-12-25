namespace RuleForge.Rules.Common
{
    public class PasswordRule : IRule<string>
    {
        private readonly PasswordOptions _options;
        public string ErrorMessage { get; }

        public PasswordRule(PasswordOptions? options = null, string? errorMessage = null)
        {
            _options = options ?? new PasswordOptions();
            ErrorMessage = errorMessage ?? GetDefaultErrorMessage();
        }

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            List<string> errors = new();

            if (value.Length < _options.MinimumLength)
            {
                errors.Add($"Password must be at least {_options.MinimumLength} characters long");
            }

            if (_options.RequireUppercase && !value.Any(char.IsUpper))
            {
                errors.Add("Password must contain at least one uppercase letter");
            }

            if (_options.RequireLowercase && !value.Any(char.IsLower))
            {
                errors.Add("Password must contain at least one lowercase letter");
            }

            if (_options.RequireDigit && !value.Any(char.IsDigit))
            {
                errors.Add("Password must contain at least one number");
            }

            if (_options.RequireSpecialCharacter && !value.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                errors.Add("Password must contain at least one special character");
            }

            if (_options.RequireNonAlphanumeric && value.All(char.IsLetterOrDigit))
            {
                errors.Add("Password must contain at least one non-alphanumeric character");
            }

            return errors.Any()
                ? ValidationResult.Error("Password", string.Join(". ", errors))
                : ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(string? value)
        {
            return Task.FromResult(Validate(value));
        }

        private string GetDefaultErrorMessage()
        {
            return "Password does not meet the requirements";
        }
    }

    public class PasswordOptions
    {
        public int MinimumLength { get; set; } = 8;
        public bool RequireUppercase { get; set; } = true;
        public bool RequireLowercase { get; set; } = true;
        public bool RequireDigit { get; set; } = true;
        public bool RequireSpecialCharacter { get; set; } = true;
        public bool RequireNonAlphanumeric { get; set; } = true;
    }
}