using RuleForge.Rules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RuleForge
{
    /// <summary>
    /// Provides a fluent interface for building validation rules.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated</typeparam>
    public class RuleBuilder<T, TProperty>
    {
        private readonly List<IRule<TProperty>> _rules = new();
        private readonly Func<T, TProperty> _propertySelector;
        private readonly string _propertyName;
        private readonly MessageFormatter _messageFormatter;
        private readonly List<Func<T, bool>> _conditions;
        private Severity _severity;

        public RuleBuilder(Func<T, TProperty> propertySelector, string propertyName)
        {
            _propertySelector = propertySelector ?? throw new ArgumentNullException(nameof(propertySelector));
            _propertyName = propertyName;
            _rules = new List<IRule<TProperty>>();
            _messageFormatter = new MessageFormatter();
            _conditions = new List<Func<T, bool>>();
            _severity = Severity.Error;
        }

        /// <summary>
        /// Sets the severity of the validation result.
        /// </summary>
        public RuleBuilder<T, TProperty> SetSeverity(Severity severity)
        {
            _severity = severity;
            return this;
        }

        /// <summary>
        /// Adds a condition to the validation process.
        /// </summary>
        public RuleBuilder<T, TProperty> When(Func<T, bool> condition)
        {
            _conditions.Add(condition);
            return this;
        }

        /// <summary>
        /// Adds a condition to the validation process that is the opposite of the provided condition.
        /// </summary>
        public RuleBuilder<T, TProperty> Unless(Func<T, bool> condition)
        {
            _conditions.Add(x => !condition(x));
            return this;
        }

        /// <summary>
        /// Sets the error message for the validation result.
        /// </summary>
        public RuleBuilder<T, TProperty> WithMessage(string message)
        {
            _messageFormatter.AddPlaceholder("PropertyName", () => _propertyName);
            _messageFormatter.AddPlaceholder("PropertyValue", () => _propertySelector);
            return this;
        }

        /// <summary>
        /// Adds a placeholder to the error message.
        /// </summary>
        public RuleBuilder<T, TProperty> WithMessagePlaceholder(string name, Func<object> valueProvider)
        {
            _messageFormatter.AddPlaceholder(name, valueProvider);
            return this;
        }

        /// <summary>
        /// Adds a rule to the builder.
        /// </summary>
        public RuleBuilder<T, TProperty> AddRule(IRule<TProperty> rule)
        {
            _rules.Add(rule);
            return this;
        }

        /// <summary>
        /// Validates the property value using all configured rules.
        /// </summary>
        public ValidationResult Validate(T instance)
        {
            if (!ShouldValidate(instance))
                return ValidationResult.Success();

            var errors = new List<ValidationError>();

            foreach (var rule in _rules)
            {
                var value = _propertySelector(instance);
                var result = rule.Validate(value);

                if (!result.IsValid)
                {
                    var formattedMessage = _messageFormatter.Format(result.ErrorMessage);
                    errors.Add(new ValidationError(_propertyName, formattedMessage));
                }
            }

            return new ValidationResult(errors, _severity);
        }

        /// <summary>
        /// Validates the property value using all configured rules synchronously.
        /// </summary>
        public async Task<ValidationResult> ValidateAsync(T instance)
        {
            if (!ShouldValidate(instance))
                return ValidationResult.Success();

            var errors = new List<ValidationError>();

            foreach (var rule in _rules)
            {
                var value = _propertySelector(instance);
                var result = await rule.ValidateAsync(value);

                if (!result.IsValid)
                {
                    var formattedMessage = _messageFormatter.Format(result.ErrorMessage);
                    errors.Add(new ValidationError(_propertyName, formattedMessage));
                }
            }

            return new ValidationResult(errors, _severity);
        }

        private bool ShouldValidate(T instance)
        {
            return _conditions.Count == 0 || _conditions.All(c => c(instance));
        }
    }
}