using System.Threading.Tasks;
using RuleForge2.Core.Validation;

namespace RuleForge2.Abstractions
{
    /// <summary>
    /// Represents a validator for a specific type.
    /// </summary>
    /// <typeparam name="T">The type to validate.</typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A validation result.</returns>
        ValidationResult Validate(T instance);

        /// <summary>
        /// Validates the specified instance asynchronously.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        Task<ValidationResult> ValidateAsync(T instance);

        /// <summary>
        /// Includes another validator.
        /// </summary>
        /// <param name="validator">The validator to include.</param>
        void Include(IValidator<T> validator);
    }
}