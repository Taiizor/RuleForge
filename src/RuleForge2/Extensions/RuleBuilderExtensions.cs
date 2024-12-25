using System;
using System.Collections.Generic;
using RuleForge2.Abstractions;
using RuleForge2.Core;
using RuleForge2.Core.Rules.Collections;
using RuleForge2.Core.Rules.Common;

namespace RuleForge2.Extensions
{
    /// <summary>
    /// Extension methods for RuleBuilder.
    /// </summary>
    public static class RuleBuilderExtensions
    {
        /// <summary>
        /// Adds a not empty rule.
        /// </summary>
        public static RuleBuilder<T, TProperty> NotEmpty<T, TProperty>(this RuleBuilder<T, TProperty> builder)
        {
            builder.AddRule(new NotEmptyRule<TProperty>(builder.PropertyName));
            return builder;
        }

        /// <summary>
        /// Adds a length rule for string properties.
        /// </summary>
        public static RuleBuilder<T, string> Length<T>(this RuleBuilder<T, string> builder, int minLength, int maxLength)
        {
            builder.AddRule(new LengthRule(builder.PropertyName, minLength, maxLength));
            return builder;
        }

        /// <summary>
        /// Adds an email rule for string properties.
        /// </summary>
        public static RuleBuilder<T, string> Email<T>(this RuleBuilder<T, string> builder)
        {
            builder.AddRule(new EmailRule(builder.PropertyName));
            return builder;
        }

        /// <summary>
        /// Adds a child validator rule.
        /// </summary>
        public static RuleBuilder<T, TChild> SetValidator<T, TChild>(this RuleBuilder<T, TChild> builder, IValidator<TChild> validator)
        {
            builder.AddRule(new ChildValidatorRule<TChild>(builder.PropertyName, validator));
            return builder;
        }

        /// <summary>
        /// Adds a collection validator rule.
        /// </summary>
        public static RuleBuilder<T, IEnumerable<TElement>> ForEach<T, TElement>(this RuleBuilder<T, IEnumerable<TElement>> builder, Action<RuleBuilder<T, TElement>> ruleConfiguration)
        {
            var itemBuilder = new RuleBuilder<T, TElement>(builder.PropertyName);
            ruleConfiguration(itemBuilder);

            var collectionRule = new CollectionRule<TElement>(builder.PropertyName, itemBuilder.Build());
            builder.AddRule(collectionRule);

            return builder;
        }
    }
}