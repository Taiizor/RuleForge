using System;
using System.Collections.Generic;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Comparison
{
    /// <summary>
    /// Rule that validates if a value is not equal to a specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    public class NotEqualRule<T> : IRule<T>
    {
        private readonly string _propertyName;
        private readonly T _comparisonValue;
        private readonly IEqualityComparer<T>? _comparer;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the NotEqualRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="comparer">Optional custom comparer.</param>
        public NotEqualRule(string propertyName, T comparisonValue, IEqualityComparer<T>? comparer = null)
        {
            _propertyName = propertyName;
            _comparisonValue = comparisonValue;
            _comparer = comparer;
            ErrorMessage = $"{propertyName} must not be equal to {comparisonValue}";
        }

        /// <summary>
        /// Validates if the value is not equal to the comparison value.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<T> context)
        {
            var isEqual = _comparer != null 
                ? _comparer.Equals(context.Instance, _comparisonValue)
                : EqualityComparer<T>.Default.Equals(context.Instance, _comparisonValue);

            if (isEqual)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates if the value is not equal to the comparison value asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            return Task.FromResult(Validate(context));
        }
    }
}