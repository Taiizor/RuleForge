using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuleForge.Core.Validation;

namespace RuleForge.Core.Rules.Collections
{
    /// <summary>
    /// Rule for validating collections of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the collection.</typeparam>
    public class CollectionRule<T> : BaseRule<IEnumerable<T>>
    {
        private readonly IRule<T> _itemRule;

        /// <summary>
        /// Initializes a new instance of the CollectionRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="itemRule">The rule to apply to each item in the collection.</param>
        public CollectionRule(string propertyName, IRule<T> itemRule)
            : base()
        {
            PropertyName = propertyName;
            _itemRule = itemRule ?? throw new ArgumentNullException(nameof(itemRule));
            ErrorMessage = $"Collection {propertyName} contains invalid items";
            ErrorMessageKey = "CollectionValidation";
        }

        /// <summary>
        /// Gets the property name being validated.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Validates each item in the collection.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public override ValidationResult Validate(ValidationContext<IEnumerable<T>> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Success();
            }

            var errors = new List<ValidationError>();
            var index = 0;

            foreach (var item in context.Instance)
            {
                var itemContext = new ValidationContext<T>(item);
                var result = _itemRule.Validate(itemContext);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        errors.Add(new ValidationError(
                            $"{PropertyName}[{index}].{error.PropertyName}",
                            error.ErrorMessage,
                            error.Severity));
                    }
                }

                index++;
            }

            return errors.Count > 0 
                ? new ValidationResult(errors) 
                : ValidationResult.Success();
        }

        /// <summary>
        /// Validates each item in the collection asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public override async Task<ValidationResult> ValidateAsync(ValidationContext<IEnumerable<T>> context)
        {
            if (context.Instance == null)
            {
                return ValidationResult.Success();
            }

            var errors = new List<ValidationError>();
            var index = 0;

            foreach (var item in context.Instance)
            {
                var itemContext = new ValidationContext<T>(item);
                var result = await _itemRule.ValidateAsync(itemContext);

                if (!result.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        errors.Add(new ValidationError(
                            $"{PropertyName}[{index}].{error.PropertyName}",
                            error.ErrorMessage,
                            error.Severity));
                    }
                }

                index++;
            }

            return errors.Count > 0 
                ? new ValidationResult(errors) 
                : ValidationResult.Success();
        }
    }
}