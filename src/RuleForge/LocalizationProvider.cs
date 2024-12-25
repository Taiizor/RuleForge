using System;
using System.Collections.Generic;
using System.Globalization;

namespace RuleForge
{
    public interface ILocalizationProvider
    {
        string GetString(string key, CultureInfo culture = null);
    }

    public class DefaultLocalizationProvider : ILocalizationProvider
    {
        private readonly Dictionary<string, Dictionary<string, string>> _translations;

        public DefaultLocalizationProvider()
        {
            _translations = new Dictionary<string, Dictionary<string, string>>();
        }

        public void AddTranslation(string key, string value, string cultureName = null)
        {
            cultureName ??= CultureInfo.InvariantCulture.Name;

            if (!_translations.ContainsKey(cultureName))
            {
                _translations[cultureName] = new Dictionary<string, string>();
            }

            _translations[cultureName][key] = value;
        }

        public string GetString(string key, CultureInfo culture = null)
        {
            culture ??= CultureInfo.CurrentCulture;

            if (_translations.TryGetValue(culture.Name, out var translations) &&
                translations.TryGetValue(key, out var value))
            {
                return value;
            }

            // Fallback to invariant culture
            if (culture.Name != CultureInfo.InvariantCulture.Name &&
                _translations.TryGetValue(CultureInfo.InvariantCulture.Name, out translations) &&
                translations.TryGetValue(key, out value))
            {
                return value;
            }

            return key;
        }
    }
}