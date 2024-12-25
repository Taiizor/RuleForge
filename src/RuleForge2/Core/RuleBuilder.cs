using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core
{
    /// <summary>
    /// Builder for creating validation rules.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    public class RuleBuilder<T, TProperty>
    {
        private readonly List<IRule<TProperty>> _rules = new List<IRule<TProperty>>();
        private readonly List<Func<T, bool>> _conditions = new List<Func<T, bool>>();

        /// <summary>
        /// Gets the name of the property being validated.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Initializes a new instance of the RuleBuilder class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        public RuleBuilder(string propertyName)
        {
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        }

        /// <summary>
        /// Adds a rule to the builder.
        /// </summary>
        /// <param name="rule">The rule to add.</param>
        public void AddRule(IRule<TProperty> rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }
            _rules.Add(rule);
        }

        /// <summary>
        /// Adds a condition that must be met for the rules to be validated.
        /// </summary>
        /// <param name="condition">The condition to add.</param>
        public RuleBuilder<T, TProperty> When(Func<T, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }
            _conditions.Add(condition);
            return this;
        }

        /// <summary>
        /// Builds a composite rule from all the added rules.
        /// </summary>
        /// <returns>A composite rule.</returns>
        public IRule<TProperty> Build()
        {
            return new CompositeRule<TProperty>(_rules);
        }

        /// <summary>
        /// Checks if all conditions are met.
        /// </summary>
        /// <param name="instance">The instance to check conditions against.</param>
        /// <returns>True if all conditions are met, false otherwise.</returns>
        public bool CheckConditions(T instance)
        {
            return _conditions.Count == 0 || _conditions.TrueForAll(c => c != null && c(instance));
        }

        /// <summary>
        /// Converts the value to the type of the property.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>The converted value.</returns>
        public TProperty ConvertValue(T value)
        {
            return (TProperty)Convert.ChangeType(value, typeof(TProperty));
        }
    }

    /// <summary>
    /// A composite rule that combines multiple rules.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    public class CompositeRule<T> : IRule<T>
    {
        private readonly IEnumerable<IRule<T>> _rules;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; } = "Validation failed";

        /// <summary>
        /// Initializes a new instance of the CompositeRule class.
        /// </summary>
        /// <param name="rules">The rules to combine.</param>
        public CompositeRule(IEnumerable<IRule<T>> rules)
        {
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
        }

        /// <summary>
        /// Validates the value against all rules.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(T value)
        {
            var validationResult = new ValidationResult();
            foreach (var rule in _rules)
            {
                if (rule != null)
                {
                    var result = rule.Validate(value);
                    if (!result.IsValid)
                    {
                        foreach (var error in result.Errors)
                        {
                            validationResult.AddError(error.PropertyName, error.ErrorMessage, error.Severity);
                        }
                    }
                }
            }

            return validationResult;
        }

        /// <summary>
        /// Validates the value against all rules asynchronously.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public async Task<ValidationResult> ValidateAsync(T value)
        {
            var validationResult = new ValidationResult();
            foreach (var rule in _rules)
            {
                if (rule != null)
                {
                    var result = await rule.ValidateAsync(value);
                    if (!result.IsValid)
                    {
                        foreach (var error in result.Errors)
                        {
                            validationResult.AddError(error.PropertyName, error.ErrorMessage, error.Severity);
                        }
                    }
                }
            }

            return validationResult;
        }
    }
}