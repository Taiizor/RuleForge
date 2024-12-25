namespace RuleForge.Rules.Common
{
    public class InclusiveBetweenRule<T> : IRule<T> where T : IComparable<T>
    {
        private readonly T _from;
        private readonly T _to;
        public string ErrorMessage { get; }

        public InclusiveBetweenRule(T from, T to, string? errorMessage = null)
        {
            _from = from;
            _to = to;
            ErrorMessage = errorMessage ?? $"Value must be between {from} and {to} (inclusive)";
        }

        public ValidationResult Validate(T? value)
        {
            if (value == null)
            {
                return ValidationResult.Success();
            }

            bool isValid = value.CompareTo(_from) >= 0 && value.CompareTo(_to) <= 0;
            return isValid
                ? ValidationResult.Success()
                : ValidationResult.Error("Range", ErrorMessage);
        }

        public Task<ValidationResult> ValidateAsync(T? value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}