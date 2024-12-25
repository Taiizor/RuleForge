using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RuleForge.Abstractions;
using RuleForge.Core;
using RuleForge.Core.Rules.Collections;
using RuleForge.Core.Rules.Common;
using RuleForge.Core.Rules.Comparison;
using RuleForge.Core.Rules.Text;
using RuleForge.Core.Validation;

namespace RuleForge.Extensions
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
        /// Adds a regular expression rule for string properties.
        /// </summary>
        public static RuleBuilder<T, string> Matches<T>(this RuleBuilder<T, string> builder, string pattern, RegexOptions options = RegexOptions.None)
        {
            builder.AddRule(new RegexRule(builder.PropertyName, pattern, options));
            return builder;
        }

        /// <summary>
        /// Adds an equal rule.
        /// </summary>
        public static RuleBuilder<T, TProperty> Equal<T, TProperty>(this RuleBuilder<T, TProperty> builder, TProperty comparisonValue, IEqualityComparer<TProperty>? comparer = null)
        {
            builder.AddRule(new EqualRule<TProperty>(builder.PropertyName, comparisonValue, comparer));
            return builder;
        }

        /// <summary>
        /// Adds a not equal rule.
        /// </summary>
        public static RuleBuilder<T, TProperty> NotEqual<T, TProperty>(this RuleBuilder<T, TProperty> builder, TProperty comparisonValue, IEqualityComparer<TProperty>? comparer = null)
        {
            builder.AddRule(new NotEqualRule<TProperty>(builder.PropertyName, comparisonValue, comparer));
            return builder;
        }

        /// <summary>
        /// Adds a greater than rule.
        /// </summary>
        public static RuleBuilder<T, TProperty> GreaterThan<T, TProperty>(this RuleBuilder<T, TProperty> builder, TProperty comparisonValue) where TProperty : IComparable<TProperty>
        {
            builder.AddRule(new GreaterThanRule<TProperty>(builder.PropertyName, comparisonValue));
            return builder;
        }

        /// <summary>
        /// Adds a less than rule.
        /// </summary>
        public static RuleBuilder<T, TProperty> LessThan<T, TProperty>(this RuleBuilder<T, TProperty> builder, TProperty comparisonValue) where TProperty : IComparable<TProperty>
        {
            builder.AddRule(new LessThanRule<TProperty>(builder.PropertyName, comparisonValue));
            return builder;
        }

        /// <summary>
        /// Adds a between rule.
        /// </summary>
        public static RuleBuilder<T, TProperty> Between<T, TProperty>(this RuleBuilder<T, TProperty> builder, TProperty from, TProperty to, bool inclusive = true) where TProperty : IComparable<TProperty>
        {
            builder.AddRule(new BetweenRule<TProperty>(builder.PropertyName, from, to, inclusive));
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

        /// <summary>
        /// Adds a validator for each element in a collection.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TElement">The type of elements in the collection.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <param name="elementValidator">The validator for collection elements.</param>
        /// <returns>The rule builder.</returns>
        public static RuleBuilder<T, IEnumerable<TElement>> ForEach<T, TElement>(
            this RuleBuilder<T, IEnumerable<TElement>> builder,
            IValidator<TElement> elementValidator)
        {
            builder.AddRule(new CollectionValidatorRule<TElement>(builder.PropertyName, elementValidator));
            return builder;
        }

        /// <summary>
        /// Adds a credit card rule for string properties.
        /// </summary>
        public static RuleBuilder<T, string> CreditCard<T>(this RuleBuilder<T, string> builder)
        {
            builder.AddRule(new CreditCardRule(builder.PropertyName));
            return builder;
        }

        /// <summary>
        /// Adds a custom validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <param name="predicate">The validation predicate.</param>
        /// <returns>The rule builder.</returns>
        public static RuleBuilder<T, TProperty> Must<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Func<TProperty, bool> predicate)
        {
            builder.AddRule(new MustRule<TProperty>(builder.PropertyName, predicate));
            return builder;
        }

        /// <summary>
        /// Adds an async custom validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <param name="predicate">The async validation predicate.</param>
        /// <returns>The rule builder.</returns>
        public static RuleBuilder<T, TProperty> MustAsync<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Func<TProperty, Task<bool>> predicate)
        {
            builder.AddRule(new MustRule<TProperty>(builder.PropertyName, predicate));
            return builder;
        }

        /// <summary>
        /// Specifies a condition that must be met for this rule to be validated.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <param name="condition">The condition that must be met.</param>
        /// <returns>The rule builder.</returns>
        public static RuleBuilder<T, TProperty> When<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Func<T, bool> condition)
        {
            var rules = builder.GetRules().ToList();
            builder.ClearRules();

            foreach (var rule in rules)
            {
                builder.AddRule(new WhenRule<TProperty>(rule, condition));
            }

            return builder;
        }

        /// <summary>
        /// Specifies a condition that must not be met for this rule to be validated.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <param name="condition">The condition that must not be met.</param>
        /// <returns>The rule builder.</returns>
        public static RuleBuilder<T, TProperty> Unless<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Func<T, bool> condition)
        {
            var rules = builder.GetRules().ToList();
            builder.ClearRules();

            foreach (var rule in rules)
            {
                builder.AddRule(new WhenRule<TProperty>(rule, condition, true));
            }

            return builder;
        }

        /// <summary>
        /// Adds a custom validation function.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <param name="validation">The custom validation function.</param>
        /// <returns>The rule builder.</returns>
        public static RuleBuilder<T, TProperty> Custom<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Action<T, ValidationContext<T>> validation)
        {
            builder.AddRule(new CustomRule<T>(builder.PropertyName, validation));
            return builder;
        }

        /// <summary>
        /// Adds an async custom validation function.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <param name="asyncValidation">The async custom validation function.</param>
        /// <returns>The rule builder.</returns>
        public static RuleBuilder<T, TProperty> CustomAsync<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            Func<T, ValidationContext<T>, Task> asyncValidation)
        {
            builder.AddRule(new CustomRule<T>(builder.PropertyName, asyncValidation));
            return builder;
        }

        /// <summary>
        /// Adds an email validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> Email<T>(this IRuleBuilder<T> ruleBuilder, string propertyName)
            where T : class
        {
            var rule = new EmailRule<string>(propertyName);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Adds a credit card validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> CreditCard<T>(this IRuleBuilder<T> ruleBuilder, string propertyName)
            where T : class
        {
            var rule = new CreditCardRule<string>(propertyName);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Adds a length validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="min">The minimum length.</param>
        /// <param name="max">The maximum length.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> Length<T>(this IRuleBuilder<T> ruleBuilder, string propertyName, int min, int max)
            where T : class
        {
            var rule = new LengthRule<string>(propertyName, min, max);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Adds a regex validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="pattern">The regex pattern.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> Matches<T>(this IRuleBuilder<T> ruleBuilder, string propertyName, string pattern)
            where T : class
        {
            var rule = new RegexRule<string>(propertyName, pattern);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Adds a not empty validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> NotEmpty<T>(this IRuleBuilder<T> ruleBuilder, string propertyName)
            where T : class
        {
            var rule = new NotEmptyRule<T>(propertyName);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Adds an equal validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to compare against.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> Equal<T, TProperty>(this IRuleBuilder<T> ruleBuilder, string propertyName, TProperty value)
            where T : class
            where TProperty : IEquatable<TProperty>
        {
            var rule = new EqualRule<TProperty>(propertyName, value);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Adds a not equal validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to compare against.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> NotEqual<T, TProperty>(this IRuleBuilder<T> ruleBuilder, string propertyName, TProperty value)
            where T : class
            where TProperty : IEquatable<TProperty>
        {
            var rule = new NotEqualRule<TProperty>(propertyName, value);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Adds a greater than validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to compare against.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> GreaterThan<T, TProperty>(this IRuleBuilder<T> ruleBuilder, string propertyName, TProperty value)
            where T : class
            where TProperty : IComparable<TProperty>
        {
            var rule = new GreaterThanRule<TProperty>(propertyName, value);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Adds a less than validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to compare against.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> LessThan<T, TProperty>(this IRuleBuilder<T> ruleBuilder, string propertyName, TProperty value)
            where T : class
            where TProperty : IComparable<TProperty>
        {
            var rule = new LessThanRule<TProperty>(propertyName, value);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }

        /// <summary>
        /// Adds a between validation rule.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
        /// <param name="ruleBuilder">The rule builder.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="from">The lower bound value.</param>
        /// <param name="to">The upper bound value.</param>
        /// <returns>The rule builder for chaining.</returns>
        public static IRuleBuilder<T> Between<T, TProperty>(this IRuleBuilder<T> ruleBuilder, string propertyName, TProperty from, TProperty to)
            where T : class
            where TProperty : IComparable<TProperty>
        {
            var rule = new BetweenRule<TProperty>(propertyName, from, to);
            ruleBuilder.AddRule(rule);
            return ruleBuilder;
        }
    }
}