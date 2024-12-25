using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace RuleForge2.Core.Localization
{
    /// <summary>
    /// Resource-based localization provider.
    /// </summary>
    public class ResourceLocalizationProvider : ILocalizationProvider
    {
        private readonly ResourceManager _resourceManager;
        private readonly CultureInfo _culture;

        /// <summary>
        /// Initializes a new instance of the ResourceLocalizationProvider class.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        /// <param name="culture">The culture to use. If null, uses the current culture.</param>
        public ResourceLocalizationProvider(ResourceManager resourceManager, CultureInfo? culture = null)
        {
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
            _culture = culture ?? CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Gets a localized message.
        /// </summary>
        /// <param name="key">The message key.</param>
        /// <param name="args">The message arguments.</param>
        /// <returns>The localized message.</returns>
        public string GetMessage(string key, IDictionary<string, object> args)
        {
            var message = _resourceManager.GetString(key, _culture);
            if (string.IsNullOrEmpty(message))
            {
                return key;
            }

            foreach (var arg in args)
            {
                message = message.Replace($"{{{arg.Key}}}", arg.Value?.ToString() ?? string.Empty);
            }

            return message;
        }

        /// <summary>
        /// Gets all available message keys.
        /// </summary>
        /// <returns>A collection of message keys.</returns>
        public IEnumerable<string> GetMessageKeys()
        {
            var resourceSet = _resourceManager.GetResourceSet(_culture, true, true);
            return resourceSet?.Cast<System.Collections.DictionaryEntry>()
                .Select(entry => entry.Key.ToString()!)
                .ToList() ?? Enumerable.Empty<string>();
        }
    }
}
