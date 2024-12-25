using System;
using System.Collections.Generic;
using System.Linq;

namespace RuleForge
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
        public bool IsValid { get; }

        /// <summary>
        /// Gets the collection of validation errors.
        /// </summary>
        public IEnumerable<ValidationError> Errors => _errors;

        /// <summary>
        /// Initializes a new instance of the ValidationResult class.
        /// </summary>
        /// <param name="isValid">Whether the validation was successful</param>
        /// <param name="errorMessage">The error message</param>
        /// <param name="severity">The severity of the validation result</param>
        public ValidationResult(bool isValid, string errorMessage = null, Severity severity = Severity.Error)
        {
            IsValid = isValid;
            _errors = new List<ValidationError>();

            if (!isValid && errorMessage != null)
            {
                _errors.Add(new ValidationError(errorMessage, severity));
            }
        }

        /// <summary>
        /// Initializes a new instance of the ValidationResult class.
        /// </summary>
        /// <param name="errors">The validation errors</param>
        public ValidationResult(IEnumerable<ValidationError> errors)
        {
            _errors = errors?.ToList() ?? new List<ValidationError>();
            IsValid = !_errors.Any();
        }

        /// <summary>
        /// Creates a successful validation result.
        /// </summary>
        public static ValidationResult Success()
        {
            return new ValidationResult(true);
        }

        /// <summary>
        /// Creates a failed validation result with the specified error.
        /// </summary>
        public static ValidationResult Error(string propertyName, string errorMessage, Severity severity = Severity.Error)
        {
            return new ValidationResult(new[] { new ValidationError(errorMessage, severity) });
        }

        /// <summary>
        /// Combines multiple validation results into a single result.
        /// </summary>
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

    /// <summary>
    /// Represents a validation error for a specific property.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Gets the severity of the validation error.
        /// </summary>
        public Severity Severity { get; }

        /// <summary>
        /// Initializes a new instance of the ValidationError class.
        /// </summary>
        public ValidationError(string errorMessage, Severity severity)
        {
            ErrorMessage = errorMessage;
            Severity = severity;
        }
    }

    public enum Severity
    {
        Info,
        Warning,
        Error
    }
}