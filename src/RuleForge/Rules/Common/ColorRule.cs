using System.Text.RegularExpressions;

namespace RuleForge.Rules.Common
{
    public class ColorRule : IRule<string>
    {
        private static readonly Regex HexColorRegex = new(
            @"^#(?:[0-9a-fA-F]{3}){1,2}$|^#(?:[0-9a-fA-F]{4}){1,2}$",
            RegexOptions.Compiled);

        private static readonly Regex RgbColorRegex = new(
            @"^rgb\(\s*(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\s*,\s*){2}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\s*\)$",
            RegexOptions.Compiled);

        private static readonly Regex RgbaColorRegex = new(
            @"^rgba\(\s*(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\s*,\s*){3}(?:0|1|0?\.[0-9]+)\s*\)$",
            RegexOptions.Compiled);

        private readonly ColorFormat _allowedFormats;
        public string ErrorMessage { get; }

        public ColorRule(ColorFormat allowedFormats = ColorFormat.All, string? errorMessage = null)
        {
            _allowedFormats = allowedFormats;
            ErrorMessage = errorMessage ?? "Invalid color format";
        }

        public ValidationResult Validate(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationResult.Success();
            }

            bool isValid = false;

            if (_allowedFormats.HasFlag(ColorFormat.Hex) && HexColorRegex.IsMatch(value))
            {
                isValid = true;
            }
            else if (_allowedFormats.HasFlag(ColorFormat.RGB) && RgbColorRegex.IsMatch(value))
            {
                isValid = true;
            }
            else if (_allowedFormats.HasFlag(ColorFormat.RGBA) && RgbaColorRegex.IsMatch(value))
            {
                isValid = true;
            }

            return isValid
                ? ValidationResult.Success()
                : ValidationResult.Error("Color", ErrorMessage);
        }

        public Task<ValidationResult> ValidateAsync(string? value)
        {
            return Task.FromResult(Validate(value));
        }
    }

    [Flags]
    public enum ColorFormat
    {
        None = 0,
        Hex = 1,
        RGB = 2,
        RGBA = 4,
        All = Hex | RGB | RGBA
    }
}