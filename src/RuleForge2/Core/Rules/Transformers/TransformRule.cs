using RuleForge2.Abstractions;
using RuleForge2.Core.Validation;

namespace RuleForge2.Core.Rules.Transformers
{
    /// <summary>
    /// Rule that transforms a value before validation.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <typeparam name="TOutput">The output type.</typeparam>
    public class TransformRule<TInput, TOutput> : IRule<TInput>
    {
        private readonly string _propertyName;
        private readonly IValueTransformer<TInput, TOutput> _transformer;
        private readonly IRule<TOutput> _innerRule;

        /// <summary>
        /// Gets or sets the error message for the rule.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the error message key for localization.
        /// </summary>
        public string? ErrorMessageKey { get; set; }

        /// <summary>
        /// Gets or sets the message formatter.
        /// </summary>
        public IMessageFormatter? MessageFormatter { get; set; }

        /// <summary>
        /// Initializes a new instance of the TransformRule class.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="transformer">The transformer to use.</param>
        /// <param name="innerRule">The rule to apply after transformation.</param>
        public TransformRule(string propertyName, IValueTransformer<TInput, TOutput> transformer, IRule<TOutput> innerRule)
        {
            _propertyName = propertyName;
            _transformer = transformer;
            _innerRule = innerRule;
            ErrorMessage = innerRule.ErrorMessage;
            ErrorMessageKey = innerRule.ErrorMessageKey;
            MessageFormatter = innerRule.MessageFormatter;
        }

        /// <summary>
        /// Validates the transformed value.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A validation result.</returns>
        public ValidationResult Validate(ValidationContext<TInput> context)
        {
            var transformedValue = _transformer.Transform(context.Instance);
            var transformedContext = new ValidationContext<TOutput>(transformedValue)
            {
                CascadeMode = context.CascadeMode,
                RootContextData = context.RootContextData
            };

            return _innerRule.Validate(transformedContext);
        }

        /// <summary>
        /// Validates the transformed value asynchronously.
        /// </summary>
        /// <param name="context">The validation context.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        public Task<ValidationResult> ValidateAsync(ValidationContext<TInput> context)
        {
            var transformedValue = _transformer.Transform(context.Instance);
            var transformedContext = new ValidationContext<TOutput>(transformedValue)
            {
                CascadeMode = context.CascadeMode,
                RootContextData = context.RootContextData
            };

            return _innerRule.ValidateAsync(transformedContext);
        }
    }
}