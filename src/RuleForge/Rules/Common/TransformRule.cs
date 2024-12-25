using System;
using System.Threading.Tasks;

namespace RuleForge.Rules.Common
{
    public class TransformRule<TProperty, TTransformed> : IRule<TProperty>
    {
        private readonly Func<TProperty, TTransformed> _transformer;
        private readonly IRule<TTransformed> _rule;

        public TransformRule(Func<TProperty, TTransformed> transformer, IRule<TTransformed> rule)
        {
            _transformer = transformer ?? throw new ArgumentNullException(nameof(transformer));
            _rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        public ValidationResult Validate(TProperty value)
        {
            var transformed = _transformer(value);
            return _rule.Validate(transformed);
        }

        public async Task<ValidationResult> ValidateAsync(TProperty value)
        {
            var transformed = _transformer(value);
            return await _rule.ValidateAsync(transformed);
        }
    }
}