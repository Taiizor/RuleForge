namespace RuleForge.Abstractions
{
    /// <summary>
    /// Interface for transforming values before validation.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <typeparam name="TOutput">The output type.</typeparam>
    public interface IValueTransformer<TInput, TOutput>
    {
        /// <summary>
        /// Transforms the input value.
        /// </summary>
        /// <param name="value">The value to transform.</param>
        /// <returns>The transformed value.</returns>
        TOutput Transform(TInput value);
    }
}