using System;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Comparison
{
    /// <summary>
    /// Rule that validates if a value is equal to a specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    public class EqualRule<T> : IRule<T>
    {
        private readonly string _propertyName;
        private readonly T _comparisonValue;
        private readonly IEqualityComparer<T>? _comparer;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the EqualRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        /// <param name="comparer">Optional custom comparer.</param>
        public EqualRule(string propertyName, T comparisonValue, IEqualityComparer<T>? comparer = null)
        {
            _propertyName = propertyName;
            _comparisonValue = comparisonValue;
            _comparer = comparer;
            ErrorMessage = $"{propertyName} must be equal to {comparisonValue}";
        }

        /// <summary>
        /// Validates if the value is equal to the comparison value.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(T value)
        {
            var isEqual = _comparer != null 
                ? _comparer.Equals(value, _comparisonValue)
                : EqualityComparer<T>.Default.Equals(value, _comparisonValue);

            if (!isEqual)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates if the value is equal to the comparison value asynchronously.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(T value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}