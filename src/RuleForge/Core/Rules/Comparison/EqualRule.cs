using System;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Comparison
{
    /// <summary>
    /// Rule that validates if a value is equal to a specified value.
    /// </summary>
    /// <typeparam name="T">The type to validate.</typeparam>
    public class EqualRule<T> : BaseRule<T> where T : IEquatable<T>
    {
        private readonly string _propertyName;
        private readonly T _comparisonValue;

        /// <summary>
        /// Initializes a new instance of the EqualRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="comparisonValue">The value to compare against.</param>
        public EqualRule(string propertyName, T comparisonValue)
        {
            _propertyName = propertyName;
            _comparisonValue = comparisonValue;
            ErrorMessage = $"{propertyName} must be equal to {comparisonValue}";
            ErrorMessageKey = "Equal";
        }

        /// <summary>
        /// Validates if the value is equal to the comparison value.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            if (!context.Instance.Equals(_comparisonValue))
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }
    }
}