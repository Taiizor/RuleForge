namespace RuleForge.Rules.Common
{
    public class NumericRangeRule<T> : IRule<T> where T : IComparable<T>
    {
        private readonly T? _minimum;
        private readonly T? _maximum;
        private readonly bool _inclusive;
        public string ErrorMessage { get; }

        public NumericRangeRule(T? minimum = default, T? maximum = default, bool inclusive = true, string? errorMessage = null)
        {
            _minimum = minimum;
            _maximum = maximum;
            _inclusive = inclusive;
            ErrorMessage = errorMessage ?? GetDefaultErrorMessage();
        }

        public ValidationResult Validate(T? value)
        {
            if (value == null)
            {
                return ValidationResult.Success();
            }

            if (_minimum != null)
            {
                int compareMin = value.CompareTo(_minimum);
                if (_inclusive && compareMin < 0 || !_inclusive && compareMin <= 0)
                {
                    return ValidationResult.Error("Range", ErrorMessage);
                }
            }

            if (_maximum != null)
            {
                int compareMax = value.CompareTo(_maximum);
                if (_inclusive && compareMax > 0 || !_inclusive && compareMax >= 0)
                {
                    return ValidationResult.Error("Range", ErrorMessage);
                }
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(T? value)
        {
            return Task.FromResult(Validate(value));
        }

        private string GetDefaultErrorMessage()
        {
            string range = _inclusive ? "[]" : "()";
            string minValue = _minimum?.ToString() ?? "-∞";
            string maxValue = _maximum?.ToString() ?? "∞";
            return $"Value must be in range {range[0]}{minValue}, {maxValue}{range[1]}";
        }
    }
}