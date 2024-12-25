using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Collections
{
    /// <summary>
    /// Rule that validates each element in a collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class CollectionRule<T> : IRule<IEnumerable<T>>
    {
        private readonly string _propertyName;
        private readonly IRule<T> _itemRule;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the CollectionRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="itemRule">The rule to apply to each item in the collection.</param>
        public CollectionRule(string propertyName, IRule<T> itemRule)
        {
            _propertyName = propertyName;
            _itemRule = itemRule;
            ErrorMessage = $"One or more items in {propertyName} are invalid";
        }

        /// <summary>
        /// Validates each element in the collection.
        /// </summary>
        /// <param name="value">The collection to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(IEnumerable<T> value)
        {
            if (value == null)
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            var results = value.Select((item, index) =>
            {
                var result = _itemRule.Validate(item);
                if (!result.IsValid)
                {
                    return ValidationResult.Error(
                        $"{_propertyName}[{index}]",
                        $"Item at index {index} is invalid: {result.Errors.First().ErrorMessage}");
                }
                return ValidationResult.Success();
            }).ToList();

            return ValidationResult.Combine(results.ToArray());
        }

        /// <summary>
        /// Validates each element in the collection asynchronously.
        /// </summary>
        /// <param name="value">The collection to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public async Task<ValidationResult> ValidateAsync(IEnumerable<T> value)
        {
            if (value == null)
            {
                return ValidationResult.Success(); // Null check should be handled by NotEmptyRule
            }

            var tasks = value.Select(async (item, index) =>
            {
                var result = await _itemRule.ValidateAsync(item);
                if (!result.IsValid)
                {
                    return ValidationResult.Error(
                        $"{_propertyName}[{index}]",
                        $"Item at index {index} is invalid: {result.Errors.First().ErrorMessage}");
                }
                return ValidationResult.Success();
            });

            var results = await Task.WhenAll(tasks);
            return ValidationResult.Combine(results);
        }
    }
}