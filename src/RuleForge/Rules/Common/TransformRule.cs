namespace RuleForge.Rules.Common
{
    public class TransformRule<TProperty, TTransformed> : IRule<TProperty>
    {
        private readonly Func<TProperty, TTransformed> _transformFunction;
        private readonly IRule<TTransformed> _rule;

        public TransformRule(Func<TProperty, TTransformed> transformFunction, IRule<TTransformed> rule)
        {
            _transformFunction = transformFunction ?? throw new ArgumentNullException(nameof(transformFunction));
            _rule = rule ?? throw new ArgumentNullException(nameof(rule));
            ErrorMessage = rule.ErrorMessage;
        }

        public string ErrorMessage { get; set; }

        public ValidationResult Validate(TProperty value)
        {
            TTransformed? transformedValue = _transformFunction(value);
            return _rule.Validate(transformedValue);
        }

        public async Task<ValidationResult> ValidateAsync(TProperty value)
        {
            TTransformed? transformedValue = _transformFunction(value);
            return await _rule.ValidateAsync(transformedValue);
        }
    }
}