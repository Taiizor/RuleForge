namespace RuleForge.Rules.Common
{
    public class GuidRule : IRule<Guid>
    {
        private readonly bool _allowEmpty;
        public string ErrorMessage { get; }

        public GuidRule(bool allowEmpty = false, string? errorMessage = null)
        {
            _allowEmpty = allowEmpty;
            ErrorMessage = errorMessage ?? "Invalid GUID value";
        }

        public ValidationResult Validate(Guid value)
        {
            if (!_allowEmpty && value == Guid.Empty)
            {
                return ValidationResult.Error("Guid", "GUID cannot be empty");
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(Guid value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}