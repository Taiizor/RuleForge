namespace RuleForge.Rules.Common
{
    public class UrlRule : IRule<string>
    {
        private readonly bool _requireHttps;
        public string ErrorMessage { get; }

        public UrlRule(bool requireHttps = false, string? errorMessage = null)
        {
            _requireHttps = requireHttps;
            ErrorMessage = errorMessage ?? "Invalid URL format";
        }

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            if (!Uri.TryCreate(value, UriKind.Absolute, out Uri? uri))
            {
                return ValidationResult.Error("URL", ErrorMessage);
            }

            if (_requireHttps && uri.Scheme != Uri.UriSchemeHttps)
            {
                return ValidationResult.Error("URL", "URL must use HTTPS");
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(string? value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}