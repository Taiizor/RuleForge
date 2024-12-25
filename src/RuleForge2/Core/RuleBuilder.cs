using System;
using System.Collections.Generic;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core
{
    /// <summary>
    /// Builder for creating validation rules.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    public class RuleBuilder<T, TProperty>
    {
        private readonly string _propertyName;
        private readonly Action<IRule<TProperty>> _ruleAdder;
        private readonly List<Func<T, bool>> _conditions;
        private readonly List<IRule<TProperty>> _rules;
        private IRule<TProperty> _currentRule;

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public string PropertyName => _propertyName;

        /// <summary>
        /// Initializes a new instance of the RuleBuilder class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="ruleAdder">Action to add a rule.</param>
        public RuleBuilder(string propertyName, Action<IRule<TProperty>> ruleAdder)
        {
            _propertyName = propertyName;
            _ruleAdder = ruleAdder;
            _conditions = new List<Func<T, bool>>();
            _rules = new List<IRule<TProperty>>();
        }

        /// <summary>
        /// Adds a condition for when the rule should be applied.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>The rule builder.</returns>
        public RuleBuilder<T, TProperty> When(Func<T, bool> condition)
        {
            _conditions.Add(condition);
            return this;
        }

        /// <summary>
        /// Adds a rule.
        /// </summary>
        /// <param name="rule">The rule to add.</param>
        public void AddRule(IRule<TProperty> rule)
        {
            _currentRule = rule;
            if (_conditions.Count > 0)
            {
                var wrappedRule = new ConditionalRule<T, TProperty>(_propertyName, rule, _conditions.ToArray());
                _ruleAdder(wrappedRule);
            }
            else
            {
                _ruleAdder(rule);
            }
            _currentRule = null;
        }

        /// <summary>
        /// Sets the severity for the current rule.
        /// </summary>
        /// <param name="severity">The severity to set.</param>
        /// <returns>The current rule builder instance.</returns>
        public RuleBuilder<T, TProperty> WithSeverity(Severity severity)
        {
            if (_currentRule != null)
            {
                _currentRule.ErrorMessage = $"{severity}: {_currentRule.ErrorMessage}";
            }
            return this;
        }

        /// <summary>
        /// Sets the error code for the current rule.
        /// </summary>
        /// <param name="errorCode">The error code to set.</param>
        /// <returns>The current rule builder instance.</returns>
        public RuleBuilder<T, TProperty> WithErrorCode(string errorCode)
        {
            if (_currentRule != null)
            {
                _currentRule.ErrorMessage = $"[{errorCode}] {_currentRule.ErrorMessage}";
            }
            return this;
        }

        /// <summary>
        /// Sets the message formatter for the current rule.
        /// </summary>
        /// <param name="formatter">The message formatter to use.</param>
        /// <returns>The current rule builder instance.</returns>
        public RuleBuilder<T, TProperty> WithMessageFormatter(IMessageFormatter formatter)
        {
            if (_currentRule != null)
            {
                _currentRule.MessageFormatter = formatter;
            }
            return this;
        }

        /// <summary>
        /// Sets the error message key for localization.
        /// </summary>
        /// <param name="messageKey">The message key to use.</param>
        /// <returns>The current rule builder instance.</returns>
        public RuleBuilder<T, TProperty> WithMessageKey(string messageKey)
        {
            if (_currentRule != null)
            {
                _currentRule.ErrorMessageKey = messageKey;
            }
            return this;
        }

        /// <summary>
        /// Transforms the value before validation.
        /// </summary>
        /// <typeparam name="TOutput">The output type.</typeparam>
        /// <param name="transformer">The transformer to use.</param>
        /// <returns>A new rule builder for the transformed value.</returns>
        public RuleBuilder<T, TOutput> Transform<TOutput>(IValueTransformer<TProperty, TOutput> transformer)
        {
            var transformRule = new TransformRule<TProperty, TOutput>(PropertyName, transformer, _currentRule);
            var newBuilder = new RuleBuilder<T, TOutput>(PropertyName, null);
            newBuilder.AddRule(transformRule);
            return newBuilder;
        }
    }

    /// <summary>
    /// Rule that wraps another rule with conditions.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    public class ConditionalRule<T, TProperty> : IRule<TProperty>
    {
        private readonly string _propertyName;
        private readonly IRule<TProperty> _rule;
        private readonly Func<T, bool>[] _conditions;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage
        {
            get => _rule.ErrorMessage;
            set => _rule.ErrorMessage = value;
        }

        /// <summary>
        /// Gets or sets the error message key for localization.
        /// </summary>
        public string ErrorMessageKey
        {
            get => _rule.ErrorMessageKey;
            set => _rule.ErrorMessageKey = value;
        }

        /// <summary>
        /// Gets or sets the message formatter for the rule.
        /// </summary>
        public IMessageFormatter MessageFormatter
        {
            get => _rule.MessageFormatter;
            set => _rule.MessageFormatter = value;
        }

        /// <summary>
        /// Initializes a new instance of the ConditionalRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="rule">The rule to wrap.</param>
        /// <param name="conditions">The conditions.</param>
        public ConditionalRule(string propertyName, IRule<TProperty> rule, Func<T, bool>[] conditions)
        {
            _propertyName = propertyName;
            _rule = rule;
            _conditions = conditions;
        }

        /// <summary>
        /// Validates the value if all conditions are met.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<TProperty> context)
        {
            if (context is ValidationContext<T> parentContext)
            {
                foreach (var condition in _conditions)
                {
                    if (!condition(parentContext.Instance))
                    {
                        return ValidationResult.Success();
                    }
                }
            }

            return _rule.Validate(context);
        }

        /// <summary>
        /// Validates the value if all conditions are met asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(ValidationContext<TProperty> context)
        {
            if (context is ValidationContext<T> parentContext)
            {
                foreach (var condition in _conditions)
                {
                    if (!condition(parentContext.Instance))
                    {
                        return Task.FromResult(ValidationResult.Success());
                    }
                }
            }

            return _rule.ValidateAsync(context);
        }
    }

    public class TransformRule<TProperty, TOutput> : IRule<TOutput>
    {
        private readonly string _propertyName;
        private readonly IValueTransformer<TProperty, TOutput> _transformer;
        private readonly IRule<TProperty> _rule;

        public string ErrorMessage { get; set; }
        public string ErrorMessageKey { get; set; }
        public IMessageFormatter MessageFormatter { get; set; }

        public TransformRule(string propertyName, IValueTransformer<TProperty, TOutput> transformer, IRule<TProperty> rule)
        {
            _propertyName = propertyName;
            _transformer = transformer;
            _rule = rule;
        }

        public ValidationResult Validate(ValidationContext<TOutput> context)
        {
            var originalValue = _transformer.TransformBack(context.Instance);
            var originalContext = new ValidationContext<TProperty>(originalValue, context.InstanceType, context.ParentContext);
            var result = _rule.Validate(originalContext);

            if (result.IsSuccess)
            {
                return ValidationResult.Success();
            }

            ErrorMessage = result.ErrorMessage;
            ErrorMessageKey = result.ErrorMessageKey;
            MessageFormatter = result.MessageFormatter;

            return result;
        }

        public Task<ValidationResult> ValidateAsync(ValidationContext<TOutput> context)
        {
            var originalValue = _transformer.TransformBack(context.Instance);
            var originalContext = new ValidationContext<TProperty>(originalValue, context.InstanceType, context.ParentContext);
            var result = _rule.ValidateAsync(originalContext);

            if (result.Result.IsSuccess)
            {
                return Task.FromResult(ValidationResult.Success());
            }

            ErrorMessage = result.Result.ErrorMessage;
            ErrorMessageKey = result.Result.ErrorMessageKey;
            MessageFormatter = result.Result.MessageFormatter;

            return Task.FromResult(result.Result);
        }
    }
}