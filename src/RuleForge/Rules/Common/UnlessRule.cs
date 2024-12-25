namespace RuleForge.Rules.Common
{
    public class UnlessRule<T, TProperty> : IRule<TProperty>
    {
        private readonly T _instance;
        private readonly Func<T, bool> _condition;
        private readonly IRule<TProperty> _rule;
        public string ErrorMessage { get; }

        public UnlessRule(
            T instance,
            Func<T, bool> condition,
            IRule<TProperty> rule,
            string? errorMessage = null)
        {
            _instance = instance;
            _condition = condition;
            _rule = rule;
            ErrorMessage = errorMessage ?? rule.ErrorMessage;
        }

        public ValidationResult Validate(TProperty? value)
        {
            if (_condition(_instance))
            {
                return ValidationResult.Success();
            }

            return _rule.Validate(value);
        }

        public async Task<ValidationResult> ValidateAsync(TProperty? value)
        {
            if (_condition(_instance))
            {
                return ValidationResult.Success();
            }

            return await _rule.ValidateAsync(value);
        }
    }
}