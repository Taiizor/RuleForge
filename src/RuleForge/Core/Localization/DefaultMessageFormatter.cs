using System;
using System.Collections.Generic;
using System.Globalization;
using RuleForge.Abstractions;

namespace RuleForge.Core.Localization
{
    /// <summary>
    /// Default implementation of IMessageFormatter.
    /// </summary>
    public class DefaultMessageFormatter : IMessageFormatter
    {
        private readonly IDictionary<string, IDictionary<string, string>> _messages;
        private readonly CultureInfo _currentCulture;

        /// <summary>
        /// Initializes a new instance of the DefaultMessageFormatter class.
        /// </summary>
        /// <param name="currentCulture">The culture to use for formatting messages.</param>
        public DefaultMessageFormatter(CultureInfo currentCulture)
        {
            _currentCulture = currentCulture ?? CultureInfo.CurrentCulture;
            _messages = new Dictionary<string, IDictionary<string, string>>();
        }

        /// <summary>
        /// Adds or updates messages for a specific culture.
        /// </summary>
        /// <param name="cultureName">The culture name (e.g., "en-US", "tr-TR").</param>
        /// <param name="messages">Dictionary of message keys and their translations.</param>
        public void AddMessages(string cultureName, IDictionary<string, string> messages)
        {
            if (string.IsNullOrEmpty(cultureName))
                throw new ArgumentNullException(nameof(cultureName));

            if (messages == null)
                throw new ArgumentNullException(nameof(messages));

            _messages[cultureName] = messages;
        }

        /// <summary>
        /// Gets a localized message using the specified key.
        /// </summary>
        /// <param name="key">The message key.</param>
        /// <param name="args">Arguments to format the message with.</param>
        /// <returns>The formatted message.</returns>
        public string GetMessage(string key, params object[] args)
        {
            var message = GetMessageTemplate(key);
            return args != null && args.Length > 0 
                ? string.Format(_currentCulture, message, args) 
                : message;
        }

        /// <summary>
        /// Gets a localized message using the specified key and placeholders.
        /// </summary>
        /// <param name="key">The message key.</param>
        /// <param name="placeholderValues">Dictionary of placeholder values.</param>
        /// <returns>The formatted message.</returns>
        public string GetMessage(string key, IDictionary<string, object> placeholderValues)
        {
            var message = GetMessageTemplate(key);

            if (placeholderValues == null || placeholderValues.Count == 0)
                return message;

            foreach (var placeholder in placeholderValues)
            {
                message = message.Replace($"{{{placeholder.Key}}}", 
                    placeholder.Value?.ToString() ?? string.Empty);
            }

            return message;
        }

        private string GetMessageTemplate(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            var cultureName = _currentCulture.Name;

            // Try to find message in current culture
            if (_messages.TryGetValue(cultureName, out var cultureMessages) &&
                cultureMessages.TryGetValue(key, out var message))
            {
                return message;
            }

            // Try to find message in parent culture
            var parentCultureName = _currentCulture.Parent.Name;
            if (!string.IsNullOrEmpty(parentCultureName) &&
                _messages.TryGetValue(parentCultureName, out var parentMessages) &&
                parentMessages.TryGetValue(key, out var parentMessage))
            {
                return parentMessage;
            }

            // Return key if no message found
            return key;
        }
    }
}