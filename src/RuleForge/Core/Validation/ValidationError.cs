namespace RuleForge.Core.Validation
{
    /// <summary>
    /// Represents a validation error.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// Gets the name of the property that failed validation.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Gets the severity of the error.
        /// </summary>
        public Severity Severity { get; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets additional error data.
        /// </summary>
        public object? ErrorData { get; set; }

        /// <summary>
        /// Initializes a new instance of the ValidationError class.
        /// </summary>
        /// <param name="propertyName">The name of the property that failed validation.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="severity">The severity of the error.</param>
        public ValidationError(string propertyName, string errorMessage, Severity severity = Severity.Error)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
            Severity = severity;
        }

        /// <summary>
        /// Creates a new validation error with an error code.
        /// </summary>
        /// <param name="propertyName">The name of the property that failed validation.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="severity">The severity of the error.</param>
        /// <returns>A new validation error.</returns>
        public static ValidationError WithErrorCode(string propertyName, string errorMessage, string errorCode, Severity severity = Severity.Error)
        {
            return new ValidationError(propertyName, errorMessage, severity)
            {
                ErrorCode = errorCode
            };
        }

        /// <summary>
        /// Creates a new validation error with additional error data.
        /// </summary>
        /// <param name="propertyName">The name of the property that failed validation.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="errorData">The error data.</param>
        /// <param name="severity">The severity of the error.</param>
        /// <returns>A new validation error.</returns>
        public static ValidationError WithErrorData(string propertyName, string errorMessage, object errorData, Severity severity = Severity.Error)
        {
            return new ValidationError(propertyName, errorMessage, severity)
            {
                ErrorData = errorData
            };
        }
    }
}