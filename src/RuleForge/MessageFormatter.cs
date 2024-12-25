using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace RuleForge
{
    public class MessageFormatter
    {
        private readonly Dictionary<string, Func<object>> _placeholderValues;
        private readonly CultureInfo _culture;

        public MessageFormatter(CultureInfo culture = null)
        {
            _placeholderValues = new Dictionary<string, Func<object>>();
            _culture = culture ?? CultureInfo.CurrentCulture;
        }

        public void AddPlaceholder(string name, Func<object> valueProvider)
        {
            _placeholderValues[name] = valueProvider;
        }

        public string Format(string message)
        {
            if (string.IsNullOrEmpty(message))
                return message;

            return Regex.Replace(message, @"\{([^{}]+)\}", match =>
            {
                string placeholder = match.Groups[1].Value;
                if (_placeholderValues.TryGetValue(placeholder, out var valueProvider))
                {
                    var value = valueProvider();
                    return FormatValue(value);
                }
                return match.Value;
            });
        }

        private string FormatValue(object value)
        {
            if (value == null)
                return string.Empty;

            if (value is IFormattable formattable)
                return formattable.ToString(null, _culture);

            return value.ToString();
        }
    }
}