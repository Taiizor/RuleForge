using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core
{
    /// <summary>
    /// Base class for validators.
    /// </summary>
    /// <typeparam name="T">The type to validate.</typeparam>
    public abstract class Validator<T> : IValidator<T>
    {
        private readonly Dictionary<string, List<RuleBuilder<T, object>>> _ruleSetMap = new Dictionary<string, List<RuleBuilder<T, object>>>();
        private readonly List<RuleBuilder<T, object>> _rules = new List<RuleBuilder<T, object>>();

        /// <summary>
        /// Creates a rule builder for a property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">Expression to get the property value.</param>
        /// <returns>A rule builder for the property.</returns>
        protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var propertyName = GetPropertyName(expression);
            var builder = new RuleBuilder<T, TProperty>(propertyName);
            _rules.Add(builder as RuleBuilder<T, object>);
            return builder;
        }

        /// <summary>
        /// Creates a rule builder for a property in a specific rule set.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">Expression to get the property value.</param>
        /// <param name="ruleSet">The rule set name.</param>
        /// <returns>A rule builder for the property.</returns>
        protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression, string ruleSet)
        {
            var propertyName = GetPropertyName(expression);
            var builder = new RuleBuilder<T, TProperty>(propertyName);

            if (!_ruleSetMap.ContainsKey(ruleSet))
            {
                _ruleSetMap[ruleSet] = new List<RuleBuilder<T, object>>();
            }

            _ruleSetMap[ruleSet].Add(builder as RuleBuilder<T, object>);
            return builder;
        }

        /// <summary>
        /// Validates an instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(T instance)
        {
            return ValidateInternal(instance, _rules);
        }

        /// <summary>
        /// Validates an instance using a specific rule set.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="ruleSet">The rule set name.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(T instance, string ruleSet)
        {
            if (!_ruleSetMap.ContainsKey(ruleSet))
            {
                throw new ArgumentException($"Rule set '{ruleSet}' does not exist.");
            }

            return ValidateInternal(instance, _ruleSetMap[ruleSet]);
        }

        /// <summary>
        /// Validates an instance asynchronously.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(T instance)
        {
            return ValidateInternalAsync(instance, _rules);
        }

        /// <summary>
        /// Validates an instance using a specific rule set asynchronously.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="ruleSet">The rule set name.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(T instance, string ruleSet)
        {
            if (!_ruleSetMap.ContainsKey(ruleSet))
            {
                throw new ArgumentException($"Rule set '{ruleSet}' does not exist.");
            }

            return ValidateInternalAsync(instance, _ruleSetMap[ruleSet]);
        }

        private ValidationResult ValidateInternal(T instance, List<RuleBuilder<T, object>> rules)
        {
            var results = new List<ValidationResult>();

            foreach (var builder in rules)
            {
                if (builder.CheckConditions(instance))
                {
                    var rule = builder.Build();
                    var result = rule.Validate(instance);
                    if (!result.IsValid)
                    {
                        results.Add(result);
                    }
                }
            }

            return ValidationResult.Combine(results.ToArray());
        }

        private async Task<ValidationResult> ValidateInternalAsync(T instance, List<RuleBuilder<T, object>> rules)
        {
            var tasks = new List<Task<ValidationResult>>();

            foreach (var builder in rules)
            {
                if (builder.CheckConditions(instance))
                {
                    var rule = builder.Build();
                    tasks.Add(rule.ValidateAsync(instance));
                }
            }

            var results = await Task.WhenAll(tasks);
            return ValidationResult.Combine(results);
        }

        private string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            throw new ArgumentException("Expression must be a member expression");
        }
    }
}