using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Common
{
    /// <summary>
    /// Rule that includes another validator.
    /// </summary>
    /// <typeparam name="T">The type to validate.</typeparam>
    public class IncludeRule<T> : IRule<T>
    {
        private readonly string _propertyName;
        private readonly IValidator<T> _validator;

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
        public IMessageFormatter? MessageFormatter
        {
            get => _validator.MessageFormatter;
            set => _validator.MessageFormatter = value;
        }

        /// <summary>
        /// Initializes a new instance of the IncludeRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="validator">The validator to include.</param>
        public IncludeRule(string propertyName, IValidator<T> validator)
        {
            _propertyName = propertyName;
            _validator = validator;
            ErrorMessage = $"Validation failed for {propertyName}";
        }

        /// <summary>
        /// Validates using the included validator.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<T> context)
        {
            _validator.CascadeMode = context.CascadeMode;
            return _validator.Validate(context.Instance);
        }

        /// <summary>
        /// Validates using the included validator asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            _validator.CascadeMode = context.CascadeMode;
            return _validator.ValidateAsync(context.Instance);
        }
    }
}