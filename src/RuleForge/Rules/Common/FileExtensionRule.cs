namespace RuleForge.Rules.Common
{
    public class FileExtensionRule : IRule<string>
    {
        private readonly HashSet<string> _allowedExtensions;
        private readonly bool _caseSensitive;
        public string ErrorMessage { get; }

        public FileExtensionRule(IEnumerable<string> allowedExtensions, bool caseSensitive = false, string? errorMessage = null)
        {
            var comparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
            _allowedExtensions = new HashSet<string>(
                allowedExtensions.Select(x => x.StartsWith(".") ? x : "." + x),
                comparer);
            _caseSensitive = caseSensitive;
            ErrorMessage = errorMessage ?? $"File extension must be one of: {string.Join(", ", _allowedExtensions)}";
        }

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            string extension = Path.GetExtension(value);
            
            if (string.IsNullOrEmpty(extension))
            {
                return ValidationResult.Error("FileExtension", "File must have an extension");
            }

            if (!_allowedExtensions.Contains(extension))
            {
                return ValidationResult.Error("FileExtension", ErrorMessage);
            }

            return ValidationResult.Success();
        }

        public Task<ValidationResult> ValidateAsync(string? value)
        {
            return Task.FromResult(Validate(value));
        }
    }
}