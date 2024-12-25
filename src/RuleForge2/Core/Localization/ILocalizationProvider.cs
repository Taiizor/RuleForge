using System.Collections.Generic;

namespace RuleForge2.Core.Localization
{
    /// <summary>
    /// Interface for localization providers.
    /// </summary>
    public interface ILocalizationProvider
    {
        /// <summary>
        /// Gets a localized message.
        /// </summary>
        /// <param name="key">The message key.</param>
        /// <param name="args">The message arguments.</param>
        /// <returns>The localized message.</returns>
        string GetMessage(string key, IDictionary<string, object> args);

        /// <summary>
        /// Gets all available message keys.
        /// </summary>
        /// <returns>A collection of message keys.</returns>
        IEnumerable<string> GetMessageKeys();
    }
}
