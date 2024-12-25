namespace RuleForge
{
    /// <summary>
    /// Represents the result of a validation operation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Gets the collection of validation errors.
        /// </summary>
        public IReadOnlyList<ValidationError> Errors { get; }

        /// <summary>
        /// Gets a value indicating whether the validation was successful.
        /// </summary>
        public bool IsValid => !Errors.Any();

        /// <summary>
        /// Gets the severity of the validation result.
        /// </summary>
        public Severity Severity { get; }

        /// <summary>
        /// Gets the custom state of the validation result.
        /// </summary>
        public IDictionary<string, object> CustomState { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string ErrorMessage => Errors.FirstOrDefault()?.ErrorMessage;

        /// <summary>
        /// Initializes a new instance of the ValidationResult class.
        /// </summary>
        /// <param name="errors">The validation errors</param>
        /// <param name="severity">The severity of the validation result</param>
        /// <param name="customState">The custom state of the validation result</param>
        public ValidationResult(IEnumerable<ValidationError> errors, Severity severity = Severity.Error, IDictionary<string, object> customState = null)
        {
            Errors = errors?.ToList() ?? new List<ValidationError>();
            Severity = severity;
            CustomState = customState ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// Creates a successful validation result.
        /// </summary>
        public static ValidationResult Success()
        {
            return new ValidationResult(Enumerable.Empty<ValidationError>());
        }

        /// <summary>
        /// Creates a failed validation result with the specified error.
        /// </summary>
        public static ValidationResult Error(string propertyName, string errorMessage, Severity severity = Severity.Error)
        {
            return new ValidationResult(new[] { new ValidationError(propertyName, errorMessage) }, severity);
        }

        /// <summary>
        /// Adds a custom state to the validation result.
        /// </summary>
        public ValidationResult WithCustomState(string key, object value)
        {
            CustomState[key] = value;
            return this;
        }
    }

    /// <summary>
    /// Represents a validation error for a specific property.
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
        /// Initializes a new instance of the ValidationError class.
        /// </summary>
        public ValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }
    }

    public enum Severity
    {
        Info,
        Warning,
        Error
    }
}