using System;
using System.Threading.Tasks;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Custom
{
    /// <summary>
    /// Rule that executes a custom validation function.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    public class CustomRule<T> : IRule<T>
    {
        private readonly string _propertyName;
        private readonly Action<T, ValidationContext<T>> _validation;
        private readonly Func<T, ValidationContext<T>, Task>? _asyncValidation;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error message key for localization.
        /// </summary>
        public string? ErrorMessageKey { get; set; }

        /// <summary>
        /// Gets or sets the message formatter.
        /// </summary>
        public IMessageFormatter? MessageFormatter { get; set; }

        /// <summary>
        /// Initializes a new instance of the CustomRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="validation">The custom validation function.</param>
        public CustomRule(string propertyName, Action<T, ValidationContext<T>> validation)
        {
            _propertyName = propertyName;
            _validation = validation;
            ErrorMessage = $"Validation failed for {propertyName}";
        }

        /// <summary>
        /// Initializes a new instance of the CustomRule class with async validation.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="asyncValidation">The async custom validation function.</param>
        public CustomRule(string propertyName, Func<T, ValidationContext<T>, Task> asyncValidation)
        {
            _propertyName = propertyName;
            _validation = null;
            _asyncValidation = asyncValidation;
            ErrorMessage = $"Validation failed for {propertyName}";
        }

        /// <summary>
        /// Executes the custom validation.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<T> context)
        {
            if (_validation == null)
            {
                throw new InvalidOperationException("Cannot use Validate with async validation. Use ValidateAsync instead.");
            }

            _validation(context.Instance, context);
            return context.Result;
        }

        /// <summary>
        /// Executes the custom validation asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public async Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            if (_asyncValidation != null)
            {
                await _asyncValidation(context.Instance, context);
            }
            else
            {
                _validation!(context.Instance, context);
            }

            return context.Result;
        }
    }
}