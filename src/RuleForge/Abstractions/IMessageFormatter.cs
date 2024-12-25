using System.Collections.Generic;

namespace RuleForge.Abstractions
{
    /// <summary>
    /// Interface for message formatting with localization support.
    /// </summary>
    public interface IMessageFormatter
    {
        /// <summary>
        /// Gets a localized message using the specified key.
        /// </summary>
        /// <param name="key">The message key.</param>
        /// <param name="args">Arguments to format the message with.</param>
        /// <returns>The formatted message.</returns>
        string GetMessage(string key, params object[] args);

        /// <summary>
        /// Gets a localized message using the specified key and placeholders.
        /// </summary>
        /// <param name="key">The message key.</param>
        /// <param name="placeholderValues">Dictionary of placeholder values.</param>
        /// <returns>The formatted message.</returns>
        string GetMessage(string key, IDictionary<string, object> placeholderValues);
    }
}