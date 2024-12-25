using System;
using System.Threading.Tasks;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Conditions
{
    /// <summary>
    /// Rule that conditionally executes another rule.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    public class WhenRule<T> : IRule<T>
    {
        private readonly IRule<T> _rule;
        private readonly Func<T, bool> _condition;
        private readonly bool _inverse;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage
        {
            get => _rule.ErrorMessage;
            set => _rule.ErrorMessage = value;
        }

        /// <summary>
        /// Gets or sets the error message key for localization.
        /// </summary>
        public string? ErrorMessageKey
        {
            get => _rule.ErrorMessageKey;
            set => _rule.ErrorMessageKey = value;
        }

        /// <summary>
        /// Gets or sets the message formatter.
        /// </summary>
        public IMessageFormatter? MessageFormatter
        {
            get => _rule.MessageFormatter;
            set => _rule.MessageFormatter = value;
        }

        /// <summary>
        /// Initializes a new instance of the WhenRule class.
        /// </summary>
        /// <param name="rule">The rule to execute conditionally.</param>
        /// <param name="condition">The condition that determines if the rule should be executed.</param>
        /// <param name="inverse">If true, inverts the condition (Unless).</param>
        public WhenRule(IRule<T> rule, Func<T, bool> condition, bool inverse = false)
        {
            _rule = rule;
            _condition = condition;
            _inverse = inverse;
        }

        /// <summary>
        /// Validates if the condition is met.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<T> context)
        {
            var shouldValidate = _condition(context.Instance);
            if (_inverse) shouldValidate = !shouldValidate;

            if (shouldValidate)
            {
                return _rule.Validate(context);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates if the condition is met asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public async Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            var shouldValidate = _condition(context.Instance);
            if (_inverse) shouldValidate = !shouldValidate;

            if (shouldValidate)
            {
                return await _rule.ValidateAsync(context);
            }

            return ValidationResult.Success();
        }
    }
}