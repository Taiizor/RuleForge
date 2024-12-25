using RuleForge.Rules.Custom;

namespace RuleForge
{
    public static class CustomRuleBuilderExtensions
    {
        public static RuleBuilder<T, TProperty> Must<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Func<TProperty, bool> predicate,
            string message)
        {
            return builder.AddRule(new CustomRule<TProperty>(predicate, message));
        }

        public static RuleBuilder<T, TProperty> MustAsync<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Func<TProperty, Task<bool>> predicate,
            string message)
        {
            return builder.AddRule(new AsyncCustomRule<TProperty>(predicate, message));
        }

        public static RuleBuilder<T, TProperty> When<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Func<T, bool> predicate,
            Action<RuleBuilder<T, TProperty>> action)
        {
            // TODO: Implement conditional validation
            throw new NotImplementedException();
        }

        public static RuleBuilder<T, TProperty> Unless<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Func<T, bool> predicate,
            Action<RuleBuilder<T, TProperty>> action)
        {
            return When(builder, x => !predicate(x), action);
        }
    }
}