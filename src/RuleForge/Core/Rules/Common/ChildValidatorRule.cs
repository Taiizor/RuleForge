using System;
using System.Threading.Tasks;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Common
{
    /// <summary>
    /// Rule that validates a child object using its own validator.
    /// </summary>
    /// <typeparam name="T">The type of the child object to validate.</typeparam>
    public class ChildValidatorRule<T> : BaseRule<T> where T : class
    {
        private readonly IValidator<T> _validator;

        /// <summary>
        /// Initializes a new instance of the ChildValidatorRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="validator">The validator for the child object.</param>
        public ChildValidatorRule(string propertyName, IValidator<T> validator)
            : base()
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            PropertyName = propertyName;
            ErrorMessage = $"Validation failed for {propertyName}";
            ErrorMessageKey = "ChildValidation";
        }

        /// <summary>
        /// Gets the property name being validated.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Validates the child object.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Success();
            }

            var result = _validator.Validate(context.Instance);
            if (!result.IsValid)
            {
                return result;
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates the child object asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public override async Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Success();
            }

            var result = await _validator.ValidateAsync(context.Instance);
            if (!result.IsValid)
            {
                return result;
            }

            return ValidationResult.Success();
        }
    }
}