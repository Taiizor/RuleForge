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
        /// Gets the validation errors.
        /// </summary>
        public IReadOnlyList<ValidationError> Errors => _errors;

        /// <summary>
        /// Gets whether the validation was successful.
        /// </summary>
        public bool IsValid => !_errors.Any();

        /// <summary>
        /// Gets whether the validation has any errors with the specified severity.
        /// </summary>
        /// <param name="severity">The severity to check for.</param>
        /// <returns>True if there are any errors with the specified severity, false otherwise.</returns>
        public bool HasErrorsWithSeverity(Severity severity)
        {
            return _errors.Any(e => e.Severity == severity);
        }

        /// <summary>
        /// Gets all errors with the specified severity.
        /// </summary>
        /// <param name="severity">The severity to filter by.</param>
        /// <returns>A list of validation errors with the specified severity.</returns>
        public IEnumerable<ValidationError> GetErrorsWithSeverity(Severity severity)
        {
            return _errors.Where(e => e.Severity == severity);
        }

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
        /// Creates a validation result with an error.
        /// </summary>
        /// <param name="propertyName">The name of the property with the error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="severity">The severity of the error.</param>
        /// <returns>A validation result with an error.</returns>
        public static ValidationResult Error(string propertyName, string errorMessage, Severity severity = Severity.Error)
        {
            var result = new ValidationResult();
            result.AddError(propertyName, errorMessage, severity);
            return result;
        }

        /// <summary>
        /// Adds an error to the validation result.
        /// </summary>
        /// <param name="propertyName">The name of the property with the error.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="severity">The severity of the error.</param>
        public void AddError(string propertyName, string errorMessage, Severity severity = Severity.Error)
        {
            _errors.Add(new ValidationError(propertyName, errorMessage, severity));
        }

        /// <summary>
        /// Merges another validation result into this one.
        /// </summary>
        /// <param name="other">The validation result to merge.</param>
        public void MergeWith(ValidationResult other)
        {
            if (other != null)
            {
                _errors.AddRange(other._errors);
            }
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
    }
}