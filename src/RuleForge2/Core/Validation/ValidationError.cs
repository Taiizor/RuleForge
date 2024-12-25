namespace RuleForge2.Core.Validation
{
    /// <summary>
    /// Represents a validation error.
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
        /// Gets the property name that caused the validation error.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Initializes a new instance of the ValidationError class.
        /// </summary>
        /// <param name="propertyName">The name of the property that failed validation.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="severity">The severity of the validation error.</param>
        public ValidationError(string propertyName, string errorMessage, Severity severity = Severity.Error)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            Severity = severity;
        }
    }
}