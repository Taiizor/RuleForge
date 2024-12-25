using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuleForge.Abstractions;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Collections
{
    /// <summary>
    /// Rule that validates each element in a collection using a validator.
    /// </summary>
    /// <typeparam name="TElement">The type of elements in the collection.</typeparam>
    public class CollectionValidatorRule<TElement> : IRule<IEnumerable<TElement>>
    {
        private readonly string _propertyName;
        private readonly IValidator<TElement> _elementValidator;

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
            get => _elementValidator.MessageFormatter;
            set => _elementValidator.MessageFormatter = value;
        }

        /// <summary>
        /// Initializes a new instance of the CollectionValidatorRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="elementValidator">The validator for collection elements.</param>
        public CollectionValidatorRule(string propertyName, IValidator<TElement> elementValidator)
        {
            _propertyName = propertyName;
            _elementValidator = elementValidator;
            ErrorMessage = $"One or more {propertyName} are invalid";
        }

        /// <summary>
        /// Validates each element in the collection.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<IEnumerable<TElement>> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Success();
            }

            var result = new ValidationResult();
            var index = 0;

            foreach (var element in context.Instance)
            {
                var elementResult = _elementValidator.Validate(element);
                if (!elementResult.IsValid)
                {
                    foreach (var error in elementResult.Errors)
                    {
                        result.AddError(
                            $"{_propertyName}[{index}].{error.PropertyName}",
                            error.ErrorMessage,
                            error.Severity);
                    }
                }
                index++;
            }

            return result;
        }

        /// <summary>
        /// Validates each element in the collection asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public async Task<ValidationResult> ValidateAsync(ValidationContext<IEnumerable<TElement>> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Success();
            }

            var result = new ValidationResult();
            var validationTasks = context.Instance.Select((element, index) =>
                ValidateElementAsync(element, index, result));

            await Task.WhenAll(validationTasks);
            return result;
        }

        private async Task ValidateElementAsync(TElement element, int index, ValidationResult result)
        {
            var elementResult = await _elementValidator.ValidateAsync(element);
            if (!elementResult.IsValid)
            {
                foreach (var error in elementResult.Errors)
                {
                    result.AddError(
                        $"{_propertyName}[{index}].{error.PropertyName}",
                        error.ErrorMessage,
                        error.Severity);
                }
            }
        }
    }
}