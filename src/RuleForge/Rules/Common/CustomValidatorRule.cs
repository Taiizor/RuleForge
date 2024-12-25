namespace RuleForge.Rules.Common
{
    public class CustomValidatorRule<T, TProperty> : IRule<TProperty>
    {
        private readonly Func<T, TProperty?, Task<ValidationResult>> _asyncValidator;
        private readonly Func<T, TProperty?, ValidationResult>? _validator;
        private readonly T _instance;
        public string ErrorMessage { get; }

        public CustomValidatorRule(
            T instance,
            Func<T, TProperty?, ValidationResult> validator,
            string? errorMessage = null)
        {
            _instance = instance;
            _validator = validator;
            _asyncValidator = (instance, value) => Task.FromResult(validator(instance, value));
            ErrorMessage = errorMessage ?? "Validation failed";
        }

        public CustomValidatorRule(
            T instance,
            Func<T, TProperty?, Task<ValidationResult>> asyncValidator,
            string? errorMessage = null)
        {
            _instance = instance;
            _asyncValidator = asyncValidator;
            ErrorMessage = errorMessage ?? "Validation failed";
        }

        public ValidationResult Validate(TProperty? value)
        {
            return _validator != null 
                ? _validator(_instance, value)
                : _asyncValidator(_instance, value).GetAwaiter().GetResult();
        }

        public Task<ValidationResult> ValidateAsync(TProperty? value)
        {
            return _asyncValidator(_instance, value);
        }
    }
}