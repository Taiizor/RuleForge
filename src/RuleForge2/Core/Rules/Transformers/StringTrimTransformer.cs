using RuleForge2.Abstractions;

namespace RuleForge2.Core.Rules.Transformers
{
    /// <summary>
    /// Transformer that trims whitespace from strings.
    /// </summary>
    public class StringTrimTransformer : IValueTransformer<string, string>
    {
        /// <summary>
        /// Transforms the input string by trimming whitespace.
        /// </summary>
        /// <param name="value">The string to transform.</param>
        /// <returns>The trimmed string.</returns>
        public string Transform(string value)
        {
            return value?.Trim();
        }
    }
}