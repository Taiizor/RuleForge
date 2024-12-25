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
        private readonly Dictionary<string, List<(string PropertyName, object Rule, Type PropertyType)>> _ruleSetMap = new Dictionary<string, List<(string PropertyName, object Rule, Type PropertyType)>>();
        private readonly List<(string PropertyName, object Rule, Type PropertyType)> _rules = new List<(string PropertyName, object Rule, Type PropertyType)>();

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
            _rules.Add((propertyName, builder, typeof(TProperty)));
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
                _ruleSetMap[ruleSet] = new List<(string PropertyName, object Rule, Type PropertyType)>();
            }

            _ruleSetMap[ruleSet].Add((propertyName, builder, typeof(TProperty)));
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

        private ValidationResult ValidateInternal(T instance, List<(string PropertyName, object Rule, Type PropertyType)> rules)
        {
            var validationResult = new ValidationResult();

            foreach (var (propertyName, rule, propertyType) in rules)
            {
                if (rule != null)
                {
                    var ruleType = rule.GetType();
                    var checkConditionsMethod = ruleType.GetMethod("CheckConditions");
                    var buildMethod = ruleType.GetMethod("Build");

                    if (checkConditionsMethod != null && buildMethod != null)
                    {
                        var conditionResult = (bool)checkConditionsMethod.Invoke(rule, new object[] { instance });
                        if (conditionResult)
                        {
                            var validationRule = buildMethod.Invoke(rule, Array.Empty<object>());
                            if (validationRule != null)
                            {
                                var validateMethod = validationRule.GetType().GetMethod("Validate");
                                if (validateMethod != null)
                                {
                                    var propertyValue = GetPropertyValue(instance, propertyName);
                                    var result = (ValidationResult)validateMethod.Invoke(validationRule, new[] { propertyValue });
                                    if (!result.IsValid)
                                    {
                                        foreach (var error in result.Errors)
                                        {
                                            validationResult.AddError(propertyName, error.ErrorMessage, error.Severity);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return validationResult;
        }

        private async Task<ValidationResult> ValidateInternalAsync(T instance, List<(string PropertyName, object Rule, Type PropertyType)> rules)
        {
            var validationResult = new ValidationResult();
            var tasks = new List<(string PropertyName, Task<ValidationResult>)>();

            foreach (var (propertyName, rule, propertyType) in rules)
            {
                if (rule != null)
                {
                    var ruleType = rule.GetType();
                    var checkConditionsMethod = ruleType.GetMethod("CheckConditions");
                    var buildMethod = ruleType.GetMethod("Build");

                    if (checkConditionsMethod != null && buildMethod != null)
                    {
                        var conditionResult = (bool)checkConditionsMethod.Invoke(rule, new object[] { instance });
                        if (conditionResult)
                        {
                            var validationRule = buildMethod.Invoke(rule, Array.Empty<object>());
                            if (validationRule != null)
                            {
                                var validateAsyncMethod = validationRule.GetType().GetMethod("ValidateAsync");
                                if (validateAsyncMethod != null)
                                {
                                    var propertyValue = GetPropertyValue(instance, propertyName);
                                    var task = (Task<ValidationResult>)validateAsyncMethod.Invoke(validationRule, new[] { propertyValue });
                                    tasks.Add((propertyName, task));
                                }
                            }
                        }
                    }
                }
            }

            foreach (var (propertyName, task) in tasks)
            {
                var result = await task;
                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        validationResult.AddError(propertyName, error.ErrorMessage, error.Severity);
                    }
                }
            }

            return validationResult;
        }

        private string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            throw new ArgumentException("Expression must be a member expression");
        }

        private object GetPropertyValue(T instance, string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found on type '{typeof(T).Name}'");
            }

            return property.GetValue(instance);
        }
    }
}