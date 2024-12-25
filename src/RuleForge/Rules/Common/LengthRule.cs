namespace RuleForge.Rules.Common
{
    public class LengthRule : IRule<string>
    {
        private readonly int _min;
        private readonly int _max;
        public string ErrorMessage { get; }

        public LengthRule(int min, int max, string errorMessage = null)
        {
            _min = min;
            _max = max;
            ErrorMessage = errorMessage ?? $"Length must be between {min} and {max}";
        }

        public ValidationResult Validate(string value)
        {
            if (value == null)
            {
                return ValidationResult.Success();
            }

            if (value.Length < _min || value.Length > _max)
            {
                return ValidationResult.Error("Length", ErrorMessage);
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(string value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}