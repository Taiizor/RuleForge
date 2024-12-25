namespace RuleForge.Rules.Common
{
    public class TimeSpanRule : IRule<TimeSpan>
    {
        private readonly TimeSpan? _minimum;
        private readonly TimeSpan? _maximum;
        private readonly bool _allowZero;
        private readonly bool _allowNegative;
        public string ErrorMessage { get; }

        public TimeSpanRule(
            TimeSpan? minimum = null,
            TimeSpan? maximum = null,
            bool allowZero = true,
            bool allowNegative = false,
            string? errorMessage = null)
        {
            _minimum = minimum;
            _maximum = maximum;
            _allowZero = allowZero;
            _allowNegative = allowNegative;
            ErrorMessage = errorMessage ?? GetDefaultErrorMessage();
        }

        public ValidationResult Validate(TimeSpan value)
        {
            if (!_allowZero && value == TimeSpan.Zero)
            {
                return ValidationResult.Error("TimeSpan", "TimeSpan cannot be zero");
            }

            if (!_allowNegative && value < TimeSpan.Zero)
            {
                return ValidationResult.Error("TimeSpan", "TimeSpan cannot be negative");
            }

            if (_minimum.HasValue && value < _minimum.Value)
            {
                return ValidationResult.Error("TimeSpan", $"TimeSpan must be greater than or equal to {_minimum.Value}");
            }

            if (_maximum.HasValue && value > _maximum.Value)
            {
                return ValidationResult.Error("TimeSpan", $"TimeSpan must be less than or equal to {_maximum.Value}");
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(TimeSpan value)
        {
            return Task.FromResult(Validate(value));
        }

        private string GetDefaultErrorMessage()
        {
            var constraints = new List<string>();

            if (_minimum.HasValue)
                constraints.Add($"minimum: {_minimum.Value}");
            if (_maximum.HasValue)
                constraints.Add($"maximum: {_maximum.Value}");
            if (!_allowZero)
                constraints.Add("non-zero");
            if (!_allowNegative)
                constraints.Add("non-negative");

            return constraints.Any()
                ? $"TimeSpan must be {string.Join(", ", constraints)}"
                : "Invalid TimeSpan value";
        }
    }
}