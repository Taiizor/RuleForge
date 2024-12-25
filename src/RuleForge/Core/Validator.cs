using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RuleForge.Core.Validation;

namespace RuleForge.Core
{
    /// <summary>
    /// Base validator class.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    public abstract class Validator<T> : IValidator<T>
    {
        private readonly List<IRule<T>> _rules;

        /// <summary>
        /// Initializes a new instance of the Validator class.
        /// </summary>
        protected Validator()
        {
            _rules = new List<IRule<T>>();
        }

        /// <summary>
        /// Adds a rule to the validator.
        /// </summary>
        /// <param name="rule">The rule to add.</param>
        protected void AddRule(IRule<T> rule)
        {
            _rules.Add(rule);
        }

        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(T instance)
        {
            var context = new ValidationContext<T>(instance);
            var errors = new List<ValidationError>();

            foreach (var rule in _rules)
            {
                var result = rule.Validate(context);
                if (!result.IsValid)
                {
                    errors.AddRange(result.Errors);
                }
            }

            return errors.Count > 0 
                ? new ValidationResult(errors) 
                : ValidationResult.Success();
        }

        /// <summary>
        /// Validates the instance asynchronously.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public async Task<ValidationResult> ValidateAsync(T instance)
        {
            var context = new ValidationContext<T>(instance);
            var errors = new List<ValidationError>();

            foreach (var rule in _rules)
            {
                var result = await rule.ValidateAsync(context);
                if (!result.IsValid)
                {
                    errors.AddRange(result.Errors);
                }
            }

            return errors.Count > 0 
                ? new ValidationResult(errors) 
                : ValidationResult.Success();
        }
    }
}