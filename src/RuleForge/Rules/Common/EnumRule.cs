namespace RuleForge.Rules.Common
{
    public class EnumRule<TEnum> : IRule<TEnum> where TEnum : struct, Enum
    {
        public string ErrorMessage { get; }

        public EnumRule(string? errorMessage = null)
        {
            ErrorMessage = errorMessage ?? $"Value must be one of the defined {typeof(TEnum).Name} values";
        }

        public ValidationResult Validate(TEnum value)
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
            {
                return ValidationResult.Error("Enum", ErrorMessage);
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(TEnum value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}