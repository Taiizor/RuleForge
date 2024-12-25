using System;
using System.Collections.Generic;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Comparison
{
    /// <summary>
    /// Rule that validates if a value is less than a specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    public class LessThanRule<T> : IRule<T> where T : IComparable<T>
    {
        private readonly string _propertyName;
        private readonly T _comparisonValue;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the LessThanRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        public LessThanRule(string propertyName, T comparisonValue)
        {
            _propertyName = propertyName;
            _comparisonValue = comparisonValue;
            ErrorMessage = $"{propertyName} must be less than {comparisonValue}";
        }

        /// <summary>
        /// Validates if the value is less than the comparison value.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<T> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            if (context.Instance.CompareTo(_comparisonValue) >= 0)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates if the value is less than the comparison value asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            return Task.FromResult(Validate(context));
        }
    }
}