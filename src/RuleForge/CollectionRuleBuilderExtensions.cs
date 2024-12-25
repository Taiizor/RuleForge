using RuleForge.Rules;
using RuleForge.Rules.Collections;

namespace RuleForge
{
    public static class CollectionRuleBuilderExtensions
    {
        public static RuleBuilder<T, IEnumerable<TElement>> ForEach<T, TElement>(
            this RuleBuilder<T, IEnumerable<TElement>> builder,
            Action<RuleBuilder<T, TElement>> action)
        {
            // Create a new builder for the element type
            RuleBuilder<T, TElement> elementBuilder = new(x => default, "Item");
            action(elementBuilder);

            // Add the collection rule that will apply the element rules
            CollectionRule<TElement> collectionRule = new(elementBuilder as IRule<TElement>);
            return builder.AddRule(collectionRule);
        }
    }
}