using System.Collections.Generic;

namespace RuleForge.Core.Validation
{
    /// <summary>
    /// Context for validation operations.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    public class ValidationContext<T>
    {
        /// <summary>
        /// Gets the instance being validated.
        /// </summary>
        public T Instance { get; }

        /// <summary>
        /// Gets the cascade mode for validation.
        /// </summary>
        public CascadeMode CascadeMode { get; }

        /// <summary>
        /// Gets the root context data dictionary.
        /// </summary>
        public IDictionary<string, object> RootContextData { get; }

        /// <summary>
        /// Gets the validation result.
        /// </summary>
        public ValidationResult Result { get; }

        /// <summary>
        /// Initializes a new instance of the ValidationContext class.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="cascadeMode">The cascade mode for validation.</param>
        public ValidationContext(T instance, CascadeMode cascadeMode)
        {
            Instance = instance;
            CascadeMode = cascadeMode;
            RootContextData = new Dictionary<string, object>();
            Result = new ValidationResult();
        }

        /// <summary>
        /// Adds a validation failure.
        /// </summary>
        /// <param name="propertyName">The name of the property that failed validation.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="severity">The severity of the failure.</param>
        public void AddFailure(string propertyName, string errorMessage, Severity severity = Severity.Error)
        {
            Result.AddError(propertyName, errorMessage, severity);
        }

        /// <summary>
        /// Adds a validation failure.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="severity">The severity of the failure.</param>
        public void AddFailure(string errorMessage, Severity severity = Severity.Error)
        {
            Result.AddError(string.Empty, errorMessage, severity);
        }
    }
}