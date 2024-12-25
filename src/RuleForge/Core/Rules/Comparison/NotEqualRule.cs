using System;
using System.Collections.Generic;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Comparison
{
    /// <summary>
    /// Rule that validates if a value is not equal to a specified value.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    public class NotEqualRule<T> : BaseRule<T> where T : IEquatable<T>
    {
        private readonly string _propertyName;
        private readonly T _comparisonValue;

        /// <summary>
        /// Initializes a new instance of the NotEqualRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        public NotEqualRule(string propertyName, T comparisonValue)
        {
            _propertyName = propertyName;
            _comparisonValue = comparisonValue;
            ErrorMessage = $"{propertyName} must not be equal to {comparisonValue}";
            ErrorMessageKey = "NotEqual";
        }

        /// <summary>
        /// Validates if the value is not equal to the comparison value.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            if (context.Instance.Equals(_comparisonValue))
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