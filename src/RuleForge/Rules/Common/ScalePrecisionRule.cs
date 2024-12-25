namespace RuleForge.Rules.Common
{
    public class ScalePrecisionRule : IRule<decimal>
    {
        private readonly int _precision;
        private readonly int _scale;
        public string ErrorMessage { get; }

        public ScalePrecisionRule(int precision, int scale, string? errorMessage = null)
        {
            if (scale > precision)
                throw new ArgumentException("Scale cannot be greater than precision", nameof(scale));

            _precision = precision;
            _scale = scale;
            ErrorMessage = errorMessage ?? $"Value must not exceed {_precision} digits in total, with allowance for {_scale} decimals";
        }

        public ValidationResult Validate(decimal value)
        {
            var parts = value.ToString("G29").Split('.');
            var integerPart = parts[0].TrimStart('-');
            var decimalPart = parts.Length > 1 ? parts[1] : "";

            int integerDigits = integerPart.Length;
            int decimalDigits = decimalPart.Length;

            if (decimalDigits > _scale)
            {
                return ValidationResult.Error("Scale", $"Decimal places cannot exceed {_scale}");
            }

            if (integerDigits + decimalDigits > _precision)
            {
                return ValidationResult.Error("Precision", $"Total number of digits cannot exceed {_precision}");
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(decimal value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}