using System.Collections.Generic;

namespace RuleForge2.Core.Localization
{
    /// <summary>
    /// Default validation messages.
    /// </summary>
    public static class Messages
    {
        /// <summary>
        /// Gets the default English messages.
        /// </summary>
        public static IDictionary<string, string> English => new Dictionary<string, string>
        {
            { "NotEmpty", "{PropertyName} must not be empty" },
            { "Length", "{PropertyName} must be between {MinLength} and {MaxLength} characters" },
            { "Email", "{PropertyName} must be a valid email address" },
            { "CreditCard", "{PropertyName} must be a valid credit card number" },
            { "NotEqual", "{PropertyName} must not be equal to {ComparisonValue}" },
            { "GreaterThan", "{PropertyName} must be greater than {ComparisonValue}" },
            { "LessThan", "{PropertyName} must be less than {ComparisonValue}" },
            { "Between", "{PropertyName} must be between {From} and {To}" },
            { "Regex", "{PropertyName} is not in the correct format" }
        };

        /// <summary>
        /// Gets the Turkish messages.
        /// </summary>
        public static IDictionary<string, string> Turkish => new Dictionary<string, string>
        {
            { "NotEmpty", "{PropertyName} boş olamaz" },
            { "Length", "{PropertyName} {MinLength} ile {MaxLength} karakter arasında olmalıdır" },
            { "Email", "{PropertyName} geçerli bir e-posta adresi olmalıdır" },
            { "CreditCard", "{PropertyName} geçerli bir kredi kartı numarası olmalıdır" },
            { "NotEqual", "{PropertyName} {ComparisonValue} değerine eşit olmamalıdır" },
            { "GreaterThan", "{PropertyName} {ComparisonValue} değerinden büyük olmalıdır" },
            { "LessThan", "{PropertyName} {ComparisonValue} değerinden küçük olmalıdır" },
            { "Between", "{PropertyName} {From} ile {To} arasında olmalıdır" },
            { "Regex", "{PropertyName} doğru formatta değil" }
        };
    }
}