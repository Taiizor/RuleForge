using System;
using System.Collections.Generic;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Comparison
{
    /// <summary>
    /// Rule that validates if a value is between two specified values.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    public class BetweenRule<T> : BaseRule<T> where T : IComparable<T>
    {
        private readonly string _propertyName;
        private readonly T _from;
        private readonly T _to;
        private readonly bool _inclusive;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the BetweenRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="from">The lower bound.</param>
        /// <param name="to">The upper bound.</param>
        /// <param name="inclusive">Whether the bounds are inclusive.</param>
        public BetweenRule(string propertyName, T from, T to, bool inclusive = true)
        {
            _propertyName = propertyName;
            _from = from;
            _to = to;
            _inclusive = inclusive;
            ErrorMessage = inclusive
                ? $"{propertyName} must be between {from} and {to} (inclusive)"
                : $"{propertyName} must be between {from} and {to} (exclusive)";
        }

        /// <summary>
        /// Validates if the value is between the specified bounds.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            var fromComparison = context.Instance.CompareTo(_from);
            var toComparison = context.Instance.CompareTo(_to);

            var isValid = _inclusive
                ? fromComparison >= 0 && toComparison <= 0
                : fromComparison > 0 && toComparison < 0;

            if (!isValid)
            {
                return ValidationResult.Error(_propertyName, ErrorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates if the value is between the specified bounds asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            return Task.FromResult(Validate(context));
        }
    }
}