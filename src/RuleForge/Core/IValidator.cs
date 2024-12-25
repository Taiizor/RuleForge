using System.Threading.Tasks;
using RuleForge.Core.Validation;

namespace RuleForge.Core
{
    /// <summary>
    /// Interface for validators.
    /// </summary>
    /// <typeparam name="T">The type being validated.</typeparam>
    public interface IValidator<T>
    {
        /// <summary>
        /// Validates the instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A validation result.</returns>
        ValidationResult Validate(T instance);

        /// <summary>
        /// Validates the instance asynchronously.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <returns>A task that represents the asynchronous validation operation.</returns>
        Task<ValidationResult> ValidateAsync(T instance);
    }
}