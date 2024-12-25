using RuleForge.Rules.Common;
using System.Linq.Expressions;

namespace RuleForge
{
    /// <summary>
    /// Base class for creating validators for specific types.
    /// </summary>
    /// <typeparam name="T">The type of object being validated</typeparam>
    public abstract class Validator<T>
    {
        private readonly List<RuleBuilder<T, object>> _rules = new();
        private readonly Dictionary<string, List<RuleBuilder<T, object>>> _ruleSets = new();
        private readonly List<IPreValidate<T>> _preValidators = new();
        private CascadeMode _cascadeMode = CascadeMode.Continue;
        private AsyncCascadeMode _asyncCascadeMode = AsyncCascadeMode.Continue;

        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        public async Task<ValidationResult> ValidateAsync(T instance)
        {
            return await ValidateInternalAsync(instance, _rules);
        }

        /// <summary>
        /// Validates the specified instance synchronously.
        /// </summary>
        public ValidationResult Validate(T instance)
        {
            return ValidateInternal(instance, _rules);
        }

        /// <summary>
        /// Validates the specified instance using the specified rule set.
        /// </summary>
        public async Task<ValidationResult> ValidateAsync(T instance, string ruleSet)
        {
            if (!_ruleSets.ContainsKey(ruleSet))
            {
                throw new ArgumentException($"RuleSet '{ruleSet}' does not exist.");
            }
            return await ValidateInternalAsync(instance, _ruleSets[ruleSet]);
        }

        /// <summary>
        /// Validates the specified instance using the specified rule set synchronously.
        /// </summary>
        public ValidationResult Validate(T instance, string ruleSet)
        {
            if (!_ruleSets.ContainsKey(ruleSet))
            {
                throw new ArgumentException($"RuleSet '{ruleSet}' does not exist.");
            }
            return ValidateInternal(instance, _ruleSets[ruleSet]);
        }

        /// <summary>
        /// Validates the specified instance with the given validation context.
        /// </summary>
        public async Task<ValidationResult> ValidateAsync(ValidationContext<T> context)
        {
            if (!ShouldValidate(context))
            {
                return ValidationResult.Success();
            }

            return await ValidateInternalAsync(context.Instance, _rules);
        }

        /// <summary>
        /// Validates the specified instance with the given validation context synchronously.
        /// </summary>
        public ValidationResult Validate(ValidationContext<T> context)
        {
            if (!ShouldValidate(context))
            {
                return ValidationResult.Success();
            }

            return ValidateInternal(context.Instance, _rules);
        }

        /// <summary>
        /// Creates a rule builder for a specific property.
        /// </summary>
        public RuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            string propertyName = GetPropertyName(expression);
            Func<T, TProperty> compiled = expression.Compile();

            RuleBuilder<T, TProperty> builder = new(compiled, propertyName);
            _rules.Add(builder as RuleBuilder<T, object>);

            return builder;
        }

        /// <summary>
        /// Creates a rule builder for a specific property and adds it to the specified rule set.
        /// </summary>
        public RuleBuilder<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression, string ruleSet)
        {
            string propertyName = GetPropertyName(expression);
            Func<T, TProperty> compiled = expression.Compile();

            RuleBuilder<T, TProperty> builder = new(compiled, propertyName);
            if (!_ruleSets.ContainsKey(ruleSet))
            {
                _ruleSets[ruleSet] = new List<RuleBuilder<T, object>>();
            }
            _ruleSets[ruleSet].Add(builder as RuleBuilder<T, object>);

            return builder;
        }

        /// <summary>
        /// Includes the rules from another validator.
        /// </summary>
        public void Include(Validator<T> validator)
        {
            _rules.AddRange(validator._rules);
            foreach (KeyValuePair<string, List<RuleBuilder<T, object>>> ruleSet in validator._ruleSets)
            {
                if (!_ruleSets.ContainsKey(ruleSet.Key))
                {
                    _ruleSets[ruleSet.Key] = new List<RuleBuilder<T, object>>();
                }
                _ruleSets[ruleSet.Key].AddRange(ruleSet.Value);
            }
            _preValidators.AddRange(validator._preValidators);
        }

        /// <summary>
        /// Sets the cascade mode for the validator.
        /// </summary>
        public void SetCascadeMode(CascadeMode mode)
        {
            _cascadeMode = mode;
        }

        /// <summary>
        /// Sets the async cascade mode for the validator.
        /// </summary>
        public void SetAsyncCascadeMode(AsyncCascadeMode mode)
        {
            _asyncCascadeMode = mode;
        }

        /// <summary>
        /// Adds a pre-validator to the validator.
        /// </summary>
        public void AddPreValidator(IPreValidate<T> preValidator)
        {
            _preValidators.Add(preValidator);
        }

        private bool ShouldValidate(ValidationContext<T> context)
        {
            return _preValidators.Count == 0 || _preValidators.All(v => v.ShouldValidate(context));
        }

        private ValidationResult ValidateInternal(T instance, List<RuleBuilder<T, object>> rules)
        {
            List<ValidationError> errors = new();
            ValidationContext<T> context = new(instance);

            if (!ShouldValidate(context))
            {
                return ValidationResult.Success();
            }

            foreach (RuleBuilder<T, object> builder in rules)
            {
                ValidationResult result = builder.Validate(instance);
                if (!result.IsValid)
                {
                    errors.AddRange(result.Errors);
                    if (_cascadeMode == CascadeMode.StopOnFirstFailure)
                    {
                        break;
                    }
                }
            }

            return new ValidationResult(errors);
        }

        private async Task<ValidationResult> ValidateInternalAsync(T instance, List<RuleBuilder<T, object>> rules)
        {
            List<ValidationError> errors = new();
            ValidationContext<T> context = new(instance);

            if (!ShouldValidate(context))
            {
                return ValidationResult.Success();
            }

            foreach (RuleBuilder<T, object> builder in rules)
            {
                ValidationResult result = await builder.ValidateAsync(instance);
                if (!result.IsValid)
                {
                    errors.AddRange(result.Errors);
                    if (_asyncCascadeMode == AsyncCascadeMode.StopOnFirstFailure)
                    {
                        break;
                    }
                }
            }

            return new ValidationResult(errors);
        }

        private string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }

            throw new ArgumentException("Expression must be a member expression", nameof(expression));
        }
    }

    public enum CascadeMode
    {
        Continue,
        StopOnFirstFailure
    }

    public enum AsyncCascadeMode
    {
        Continue,
        StopOnFirstFailure
    }
}