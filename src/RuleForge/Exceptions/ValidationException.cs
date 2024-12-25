namespace RuleForge.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationResult ValidationResult { get; }
        public IEnumerable<ValidationError> Errors => ValidationResult.Errors;
        public string ErrorMessage => ValidationResult.ErrorMessage;

        public ValidationException(ValidationResult validationResult)
            : base(validationResult.ErrorMessage)
        {
            ValidationResult = validationResult;
        }

        public ValidationException(string message, ValidationResult validationResult)
            : base(message)
        {
            ValidationResult = validationResult;
        }

        public ValidationException(string message, ValidationResult validationResult, Exception innerException)
            : base(message, innerException)
        {
            ValidationResult = validationResult;
        }

        public override string ToString()
        {
            return $"Validation failed: {string.Join(Environment.NewLine, Errors.Select(e => e.ErrorMessage))}";
        }
    }
}