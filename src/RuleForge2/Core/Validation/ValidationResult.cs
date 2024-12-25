using System.Collections.Generic;
using System.Linq;

namespace RuleForge2.Core.Validation
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        private readonly List<ValidationError> _errors;

        /// <summary>
        /// Gets a value indicating whether the validation was successful.
        /// </summary>
        public bool IsValid => !_errors.Any();

        /// <summary>
        /// Gets the collection of validation errors.
        /// </summary>
        public IReadOnlyList<ValidationError> Errors => _errors;

        /// <summary>
        /// Initializes a new instance of the ValidationResult class.
        /// </summary>
        public ValidationResult()
        {
            _errors = new List<ValidationError>();
        }

        /// <summary>
        /// Initializes a new instance of the ValidationResult class with the specified errors.
        /// </summary>
        /// <param name="errors">The validation errors.</param>
        public ValidationResult(IEnumerable<ValidationError> errors)
        {
            _errors = errors?.ToList() ?? new List<ValidationError>();
        }

        /// <summary>
        /// Creates a successful validation result.
        /// </summary>
        /// <returns>A successful validation result.</returns>
        public static ValidationResult Success()
        {
            return new ValidationResult();
        }

        /// <summary>
        /// Creates a failed validation result with a single error.
        /// </summary>
        /// <param name="propertyName">The name of the property that failed validation.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="severity">The severity of the validation error.</param>
        /// <returns>A failed validation result.</returns>
        public static ValidationResult Error(string propertyName, string errorMessage, Severity severity = Severity.Error)
        {
            return new ValidationResult(new[] { new ValidationError(propertyName, errorMessage, severity) });
        }

        /// <summary>
        /// Combines multiple validation results into a single result.
        /// </summary>
        /// <param name="results">The validation results to combine.</param>
        /// <returns>A combined validation result.</returns>
        public static ValidationResult Combine(params ValidationResult[] results)
        {
            if (results == null || results.Length == 0)
            {
                return Success();
            }

            var errors = results.SelectMany(r => r.Errors).ToList();
            return new ValidationResult(errors);
        }

        /// <summary>
        /// Adds a validation error to this result.
        /// </summary>
        /// <param name="propertyName">The name of the property that failed validation.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="severity">The severity of the validation error.</param>
        public void AddError(string propertyName, string errorMessage, Severity severity = Severity.Error)
        {
            _errors.Add(new ValidationError(propertyName, errorMessage, severity));
        }
    }
}