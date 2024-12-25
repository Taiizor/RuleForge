namespace RuleForge.Rules.Common
{
    public class ComparisonRule<T> : IRule<T> where T : IComparable<T>
    {
        private readonly T _comparisonValue;
        private readonly ComparisonType _comparisonType;
        public string ErrorMessage { get; }

        public ComparisonRule(T comparisonValue, ComparisonType comparisonType, string errorMessage = null)
        {
            _comparisonValue = comparisonValue;
            _comparisonType = comparisonType;
            ErrorMessage = errorMessage ?? GetDefaultErrorMessage();
        }

        public ValidationResult Validate(T value)
        {
            if (value == null)
            {
                return ValidationResult.Success();
            }

            bool isValid = _comparisonType switch
            {
                ComparisonType.LessThan => value.CompareTo(_comparisonValue) < 0,
                ComparisonType.LessThanOrEqual => value.CompareTo(_comparisonValue) <= 0,
                ComparisonType.GreaterThan => value.CompareTo(_comparisonValue) > 0,
                ComparisonType.GreaterThanOrEqual => value.CompareTo(_comparisonValue) >= 0,
                ComparisonType.Equal => value.CompareTo(_comparisonValue) == 0,
                ComparisonType.NotEqual => value.CompareTo(_comparisonValue) != 0,
                _ => throw new ArgumentException("Invalid comparison type")
            };

            return isValid ? ValidationResult.Success() : ValidationResult.Error("Comparison", ErrorMessage);
        }

        public Task<ValidationResult> ValidateAsync(T value)
        {
            return Task.FromResult(Validate(value));
        }

        private string GetDefaultErrorMessage()
        {
            return _comparisonType switch
            {
                ComparisonType.LessThan => $"Value must be less than {_comparisonValue}",
                ComparisonType.LessThanOrEqual => $"Value must be less than or equal to {_comparisonValue}",
                ComparisonType.GreaterThan => $"Value must be greater than {_comparisonValue}",
                ComparisonType.GreaterThanOrEqual => $"Value must be greater than or equal to {_comparisonValue}",
                ComparisonType.Equal => $"Value must be equal to {_comparisonValue}",
                ComparisonType.NotEqual => $"Value must not be equal to {_comparisonValue}",
                _ => "Invalid comparison"
            };
        }
    }

    public enum ComparisonType
    {
        LessThan,
        LessThanOrEqual,
        GreaterThan,
        GreaterThanOrEqual,
        Equal,
        NotEqual
    }
}