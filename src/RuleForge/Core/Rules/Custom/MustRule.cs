using System;
using System.Threading.Tasks;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Custom
{
    /// <summary>
    /// Rule that validates using a custom predicate.
    /// </summary>
    /// <typeparam name="T">The type to validate.</typeparam>
    public class MustRule<T> : IRule<T>
    {
        private readonly string _propertyName;
        private readonly Func<T, bool> _predicate;
        private readonly Func<T, Task<bool>>? _asyncPredicate;

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
        /// Initializes a new instance of the MustRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="predicate">The validation predicate.</param>
        public MustRule(string propertyName, Func<T, bool> predicate)
        {
            _propertyName = propertyName;
            _predicate = predicate;
            ErrorMessage = $"{propertyName} is invalid";
        }

        /// <summary>
        /// Initializes a new instance of the MustRule class with an async predicate.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="asyncPredicate">The async validation predicate.</param>
        public MustRule(string propertyName, Func<T, Task<bool>> asyncPredicate)
        {
            _propertyName = propertyName;
            _predicate = null;
            _asyncPredicate = asyncPredicate;
            ErrorMessage = $"{propertyName} is invalid";
        }

        /// <summary>
        /// Validates using the custom predicate.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<T> context)
        {
            if (_predicate == null)
            {
                throw new InvalidOperationException("Cannot use Validate with async predicate. Use ValidateAsync instead.");
            }

            if (!_predicate(context.Instance))
            {
                var errorMessage = MessageFormatter != null && !string.IsNullOrEmpty(ErrorMessageKey)
                    ? MessageFormatter.GetMessage(ErrorMessageKey, new Dictionary<string, object>
                    {
                        { "PropertyName", _propertyName },
                        { "PropertyValue", context.Instance?.ToString() ?? "null" }
                    })
                    : ErrorMessage;

                return ValidationResult.Error(_propertyName, errorMessage);
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates using the custom predicate asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public async Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            var isValid = _asyncPredicate != null 
                ? await _asyncPredicate(context.Instance) 
                : _predicate(context.Instance);

            if (!isValid)
            {
                var errorMessage = MessageFormatter != null && !string.IsNullOrEmpty(ErrorMessageKey)
                    ? MessageFormatter.GetMessage(ErrorMessageKey, new Dictionary<string, object>
                    {
                        { "PropertyName", _propertyName },
                        { "PropertyValue", context.Instance?.ToString() ?? "null" }
                    })
                    : ErrorMessage;

                return ValidationResult.Error(_propertyName, errorMessage);
            }

            return ValidationResult.Success();
        }
    }
}