using System.Globalization;
using RuleForge.Abstractions;

namespace RuleForge.Core.Rules.Transformers
{
    /// <summary>
    /// Transformer that converts strings to lowercase.
    /// </summary>
    public class ToLowerTransformer : IValueTransformer<string, string>
    {
        private readonly CultureInfo _culture;

        /// <summary>
        /// Initializes a new instance of the ToLowerTransformer class.
        /// </summary>
        /// <param name="culture">The culture to use for case conversion.</param>
        public ToLowerTransformer(CultureInfo? culture = null)
        {
            _culture = culture ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Transforms the input string to lowercase.
        /// </summary>
        /// <param name="value">The string to transform.</param>
        /// <returns>The lowercase string.</returns>
        public string Transform(string value)
        {
            return value?.ToLower(_culture);
        }
    }
}