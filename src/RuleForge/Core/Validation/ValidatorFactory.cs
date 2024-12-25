using System;
using System.Collections.Generic;
using RuleForge.Abstractions;

namespace RuleForge.Core.Validation
{
    /// <summary>
    /// Factory for creating validator instances.
    /// </summary>
    public class ValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, Type> _validatorTypes;

        /// <summary>
        /// Initializes a new instance of the ValidatorFactory class.
        /// </summary>
        /// <param name="serviceProvider">The service provider for dependency injection.</param>
        public ValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _validatorTypes = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Registers a validator type.
        /// </summary>
        /// <typeparam name="TModel">The type to validate.</typeparam>
        /// <typeparam name="TValidator">The validator type.</typeparam>
        public void Register<TModel, TValidator>() where TValidator : IValidator<TModel>
        {
            _validatorTypes[typeof(TModel)] = typeof(TValidator);
        }

        /// <summary>
        /// Gets a validator instance for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to validate.</typeparam>
        /// <returns>A validator instance.</returns>
        public IValidator<T> GetValidator<T>()
        {
            var modelType = typeof(T);
            if (!_validatorTypes.TryGetValue(modelType, out var validatorType))
            {
                throw new InvalidOperationException($"No validator registered for type {modelType.Name}");
            }

            return (IValidator<T>)ActivatorUtilities.CreateInstance(_serviceProvider, validatorType);
        }

        /// <summary>
        /// Gets a validator instance for the specified type.
        /// </summary>
        /// <param name="modelType">The type to validate.</param>
        /// <returns>A validator instance.</returns>
        public IValidator<object> GetValidator(Type modelType)
        {
            if (!_validatorTypes.TryGetValue(modelType, out var validatorType))
            {
                throw new InvalidOperationException($"No validator registered for type {modelType.Name}");
            }

            return (IValidator<object>)ActivatorUtilities.CreateInstance(_serviceProvider, validatorType);
        }
    }

    /// <summary>
    /// Helper class for creating instances with dependencies.
    /// </summary>
    internal static class ActivatorUtilities
    {
        /// <summary>
        /// Creates an instance of the specified type.
        /// </summary>
        /// <param name="provider">The service provider.</param>
        /// <param name="instanceType">The type to create.</param>
        /// <returns>The created instance.</returns>
        public static object CreateInstance(IServiceProvider provider, Type instanceType)
        {
            var constructor = instanceType.GetConstructors()[0];
            var parameters = constructor.GetParameters();
            var arguments = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var service = provider.GetService(parameters[i].ParameterType);
                if (service == null)
                {
                    throw new InvalidOperationException(
                        $"Unable to resolve service for type {parameters[i].ParameterType.Name}");
                }
                arguments[i] = service;
            }

            return constructor.Invoke(arguments);
        }
    }
}