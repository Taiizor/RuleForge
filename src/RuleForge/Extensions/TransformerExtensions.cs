using System.Globalization;
using RuleForge.Abstractions;
using RuleForge.Core;
using RuleForge.Core.Rules.Transformers;

namespace RuleForge.Extensions
{
    /// <summary>
    /// Extension methods for transforming values before validation.
    /// </summary>
    public static class TransformerExtensions
    {
        /// <summary>
        /// Transforms the value before validation.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <typeparam name="TProperty">The property type.</typeparam>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <param name="transformer">The transformer to use.</param>
        /// <returns>A new rule builder for the transformed value.</returns>
        public static RuleBuilder<T, TOutput> Transform<T, TProperty, TOutput>(
            this RuleBuilder<T, TProperty> builder,
            IValueTransformer<TProperty, TOutput> transformer)
        {
            return new RuleBuilder<T, TOutput>(builder.PropertyName);
        }

        /// <summary>
        /// Trims whitespace from the string before validation.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <returns>The rule builder.</returns>
        public static RuleBuilder<T, string> Trim<T>(this RuleBuilder<T, string> builder)
        {
            var transformer = new StringTrimTransformer();
            return builder.Transform(transformer);
        }

        /// <summary>
        /// Converts the string to lowercase before validation.
        /// </summary>
        /// <typeparam name="T">The model type.</typeparam>
        /// <param name="builder">The rule builder.</param>
        /// <param name="culture">The culture to use for case conversion.</param>
        /// <returns>The rule builder.</returns>
        public static RuleBuilder<T, string> ToLower<T>(this RuleBuilder<T, string> builder, CultureInfo? culture = null)
        {
            var transformer = new ToLowerTransformer(culture);
            return builder.Transform(transformer);
        }
    }
}