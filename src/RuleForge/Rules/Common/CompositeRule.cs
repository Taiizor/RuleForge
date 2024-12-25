namespace RuleForge.Rules.Common
{
    public class CompositeRule<T> : IRule<T>
    {
        private readonly IRule<T>[] _rules;
        private readonly CompositeRuleOperator _operator;
        public string ErrorMessage { get; }

        public CompositeRule(
            IEnumerable<IRule<T>> rules,
            CompositeRuleOperator @operator = CompositeRuleOperator.And,
            string? errorMessage = null)
        {
            _rules = rules.ToArray();
            _operator = @operator;
            ErrorMessage = errorMessage ?? GetDefaultErrorMessage();
        }

        public ValidationResult Validate(T? value)
        {
            var results = _rules.Select(rule => rule.Validate(value)).ToList();

            if (_operator == CompositeRuleOperator.And)
            {
                if (results.All(r => r.IsValid))
                {
                    return ValidationResult.Success();
                }

                var firstError = results.FirstOrDefault(r => !r.IsValid);
                return firstError ?? ValidationResult.Error("Composite", ErrorMessage);
            }
            else // Or operator
            {
                if (results.Any(r => r.IsValid))
                {
                    return ValidationResult.Success();
                }

                var firstError = results.FirstOrDefault();
                return firstError ?? ValidationResult.Error("Composite", ErrorMessage);
            }
        }

        public async Task<ValidationResult> ValidateAsync(T? value)
        {
            var tasks = _rules.Select(rule => rule.ValidateAsync(value));
            var results = await Task.WhenAll(tasks);

            if (_operator == CompositeRuleOperator.And)
            {
                if (results.All(r => r.IsValid))
                {
                    return ValidationResult.Success();
                }

                var firstError = results.FirstOrDefault(r => !r.IsValid);
                return firstError ?? ValidationResult.Error("Composite", ErrorMessage);
            }
            else // Or operator
            {
                if (results.Any(r => r.IsValid))
                {
                    return ValidationResult.Success();
                }

                var firstError = results.FirstOrDefault();
                return firstError ?? ValidationResult.Error("Composite", ErrorMessage);
            }
        }

        private string GetDefaultErrorMessage()
        {
            return _operator == CompositeRuleOperator.And
                ? "All validation rules must pass"
                : "At least one validation rule must pass";
        }
    }

    public enum CompositeRuleOperator
    {
        And,
        Or
    }
}