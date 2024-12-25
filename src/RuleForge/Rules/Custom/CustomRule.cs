namespace RuleForge.Rules.Custom
{
    public class CustomRule<T> : IRule<T>
    {
        private readonly Func<T, bool> _predicate;
        public string ErrorMessage { get; }

        public CustomRule(Func<T, bool> predicate, string errorMessage)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            ErrorMessage = errorMessage ?? "Validation failed";
        }

        public ValidationResult Validate(T value)
        {
            return _predicate(value)
                ? ValidationResult.Success()
                : ValidationResult.Error("Custom", ErrorMessage);
        }

        public Task<ValidationResult> ValidateAsync(T value)
        {
            return Task.FromResult(Validate(value));
        }
    }

    public class AsyncCustomRule<T> : IRule<T>
    {
        private readonly Func<T, Task<bool>> _predicate;
        public string ErrorMessage { get; }

        public AsyncCustomRule(Func<T, Task<bool>> predicate, string errorMessage)
        {
            _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
            ErrorMessage = errorMessage ?? "Validation failed";
        }

        public ValidationResult Validate(T value)
        {
            throw new NotSupportedException("This rule only supports async validation");
        }

        public async Task<ValidationResult> ValidateAsync(T value)
        {
            return await _predicate(value)
                ? ValidationResult.Success()
                : ValidationResult.Error("Custom", ErrorMessage);
        }
    }
}