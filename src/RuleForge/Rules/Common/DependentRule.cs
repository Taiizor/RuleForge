namespace RuleForge.Rules.Common
{
    public class DependentRule<T, TProperty, TDependency> : IRule<TProperty>
    {
        private readonly T _instance;
        private readonly Func<T, TDependency> _dependencySelector;
        private readonly Func<TProperty?, TDependency, ValidationResult> _validator;
        public string ErrorMessage { get; }

        public DependentRule(
            T instance,
            Func<T, TDependency> dependencySelector,
            Func<TProperty?, TDependency, ValidationResult> validator,
            string? errorMessage = null)
        {
            _instance = instance;
            _dependencySelector = dependencySelector;
            _validator = validator;
            ErrorMessage = errorMessage ?? "Validation failed";
        }

        public ValidationResult Validate(TProperty? value)
        {
            var dependentValue = _dependencySelector(_instance);
            return _validator(value, dependentValue);
        }

        public Task<ValidationResult> ValidateAsync(TProperty? value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}