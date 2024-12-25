using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core
{
    /// <summary>
    /// Base class for validators.
    /// </summary>
    /// <typeparam name="T">The type to validate.</typeparam>
    public abstract class Validator<T> : IValidator<T>
    {
        private readonly Dictionary<string, RuleSet<T>> _ruleSets;
        private readonly RuleSet<T> _defaultRuleSet;
        private CascadeMode _cascadeMode;
        private IMessageFormatter? _messageFormatter;
        private string _currentRuleSet;

        /// <summary>
        /// Gets or sets the cascade mode for all rules.
        /// </summary>
        public CascadeMode CascadeMode
        {
            get => _cascadeMode;
            set
            {
                _cascadeMode = value;
                foreach (var ruleSet in _ruleSets.Values)
                {
                    foreach (var rule in ruleSet.Rules)
                    {
                        if (rule is IRuleWithCascadeMode cascadeRule)
                        {
                            cascadeRule.CascadeMode = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the message formatter for all rules.
        /// </summary>
        public IMessageFormatter? MessageFormatter
        {
            get => _messageFormatter;
            set
            {
                _messageFormatter = value;
                foreach (var ruleSet in _ruleSets.Values)
                {
                    foreach (var rule in ruleSet.Rules)
                    {
                        rule.MessageFormatter = value;
                    }
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the Validator class.
        /// </summary>
        protected Validator()
        {
            _ruleSets = new Dictionary<string, RuleSet<T>>();
            _defaultRuleSet = new RuleSet<T>("default");
            _ruleSets.Add(_defaultRuleSet.Name, _defaultRuleSet);
            _currentRuleSet = _defaultRuleSet.Name;
            _cascadeMode = CascadeMode.Continue;
        }

        /// <summary>
        /// Creates a rule builder for a property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="expression">The property expression.</param>
        /// <returns>A rule builder.</returns>
        protected RuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            var propertyName = expression.Body.ToString();
            return new RuleBuilder<T, TProperty>(propertyName, rule =>
            {
                var currentRuleSet = _ruleSets[_currentRuleSet];
                currentRuleSet.Rules.Add(rule);
            });
        }

        /// <summary>
        /// Creates a new rule set.
        /// </summary>
        /// <param name="name">The name of the rule set.</param>
        /// <param name="action">The action to configure rules in the set.</param>
        protected void RuleSet(string name, Action action)
        {
            if (!_ruleSets.ContainsKey(name))
            {
                _ruleSets.Add(name, new RuleSet<T>(name));
            }

            var previousRuleSet = _currentRuleSet;
            _currentRuleSet = name;

            action();

            _currentRuleSet = previousRuleSet;
        }

        /// <summary>
        /// Includes another validator.
        /// </summary>
        /// <param name="validator">The validator to include.</param>
        public void Include(IValidator<T> validator)
        {
            var includeRule = new IncludeRule<T>("", validator);
            _defaultRuleSet.Rules.Add(includeRule);
        }

        /// <summary>
        /// Called before validation begins.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>True to continue validation, false to stop.</returns>
        protected virtual bool PreValidate(ValidationContext<T> context)
        {
            return true;
        }

        /// <summary>
        /// Called before validation begins asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected virtual Task<bool> PreValidateAsync(ValidationContext<T> context)
        {
            return Task.FromResult(PreValidate(context));
        }

        /// <summary>
        /// Validates an instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="ruleSet">Optional rule set name to validate against.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(T instance, string? ruleSet = null)
        {
            var context = new ValidationContext<T>(instance)
            {
                CascadeMode = _cascadeMode
            };

            if (!PreValidate(context))
            {
                return new ValidationResult();
            }

            var ruleSetsToExecute = GetRuleSetsToExecute(ruleSet);
            var result = new ValidationResult();

            foreach (var set in ruleSetsToExecute)
            {
                foreach (var rule in set.Rules)
                {
                    var ruleResult = rule.Validate(context);
                    result.MergeWith(ruleResult);

                    if (!ruleResult.IsValid && context.CascadeMode == CascadeMode.StopOnFirstFailure)
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Validates an instance asynchronously.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="ruleSet">Optional rule set name to validate against.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public async Task<ValidationResult> ValidateAsync(T instance, string? ruleSet = null)
        {
            var context = new ValidationContext<T>(instance)
            {
                CascadeMode = _cascadeMode
            };

            if (!await PreValidateAsync(context))
            {
                return new ValidationResult();
            }

            var ruleSetsToExecute = GetRuleSetsToExecute(ruleSet);
            var result = new ValidationResult();

            foreach (var set in ruleSetsToExecute)
            {
                foreach (var rule in set.Rules)
                {
                    var ruleResult = await rule.ValidateAsync(context);
                    result.MergeWith(ruleResult);

                    if (!ruleResult.IsValid && context.CascadeMode == CascadeMode.StopOnFirstFailure)
                    {
                        return result;
                    }
                }
            }

            return result;
        }

        private IEnumerable<RuleSet<T>> GetRuleSetsToExecute(string? ruleSet)
        {
            if (string.IsNullOrEmpty(ruleSet))
            {
                yield return _defaultRuleSet;
                yield break;
            }

            var ruleSetNames = ruleSet.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var name in ruleSetNames)
            {
                if (_ruleSets.TryGetValue(name, out var set))
                {
                    yield return set;
                }
            }
        }
    }
}