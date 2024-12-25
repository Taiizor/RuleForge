using System;
using System.Threading.Tasks;
using RuleForge.Core.Validation;

namespace RuleForge.Core
{
    /// <summary>
    /// Interface for building validation rules.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    /// <typeparam name="TProperty">The type of the property being validated.</typeparam>
    public interface IRuleBuilder<T, TProperty>
    {
        /// <summary>
        /// Gets the validation rule.
        /// </summary>
        IRule<TProperty> Rule { get; }

        /// <summary>
        /// Sets the error message for the rule.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <returns>The rule builder.</returns>
        IRuleBuilder<T, TProperty> WithMessage(string message);

        /// <summary>
        /// Sets the error message key for the rule.
        /// </summary>
        /// <param name="key">The error message key.</param>
        /// <returns>The rule builder.</returns>
        IRuleBuilder<T, TProperty> WithMessageKey(string key);

        /// <summary>
        /// Sets the message formatter for the rule.
        /// </summary>
        /// <param name="formatter">The message formatter.</param>
        /// <returns>The rule builder.</returns>
        IRuleBuilder<T, TProperty> WithFormatter(Func<string, object[], string> formatter);

        /// <summary>
        /// Sets the severity for the rule.
        /// </summary>
        /// <param name="severity">The severity level.</param>
        /// <returns>The rule builder.</returns>
        IRuleBuilder<T, TProperty> WithSeverity(Severity severity);
    }
}