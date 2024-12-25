namespace RuleForge.Rules.Common
{
    public class NotEmptyRule<T> : IRule<T>
    {
        public string ErrorMessage { get; }

        public NotEmptyRule(string errorMessage = null)
        {
            ErrorMessage = errorMessage ?? "Value cannot be empty";
        }

        public ValidationResult Validate(T value)
        {
            if (value == null)
            {
                return ValidationResult.Error("Value", ErrorMessage);
            }

            if (value is string str && string.IsNullOrWhiteSpace(str))
            {
                return ValidationResult.Error("Value", ErrorMessage);
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(T value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}