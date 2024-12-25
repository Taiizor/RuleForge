using System.Threading.Tasks;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Common
{
    /// <summary>
    /// Rule that validates a child object using its own validator.
    /// </summary>
    /// <typeparam name="T">The type of the child object to validate.</typeparam>
    public class ChildValidatorRule<T> : IRule<T>
    {
        private readonly string _propertyName;
        private readonly IValidator<T> _validator;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the ChildValidatorRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="validator">The validator for the child object.</param>
        public ChildValidatorRule(string propertyName, IValidator<T> validator)
        {
            _propertyName = propertyName;
            _validator = validator;
            ErrorMessage = $"{propertyName} is invalid";
        }

        /// <summary>
        /// Validates the child object using its validator.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(T value)
        {
            if (value == null)
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            var result = _validator.Validate(value);
            if (!result.IsValid)
            {
                return result;
            }

            return ValidationResult.Success();
        }

        /// <summary>
        /// Validates the child object using its validator asynchronously.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public async Task<ValidationResult> ValidateAsync(T value)
        {
            if (value == null)
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            var result = await _validator.ValidateAsync(value);
            if (!result.IsValid)
            {
                return result;
            }

            return ValidationResult.Success();
        }
    }
}