using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace RuleForge.DependencyInjectionExtensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all validators from the specified assemblies to the service collection.
        /// </summary>
        public static IServiceCollection AddRuleForgeValidators(this IServiceCollection services, params Assembly[] assemblies)
        {
            Type validatorType = typeof(Validator<>);

            IEnumerable<Type> validatorTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && IsValidator(t, validatorType));

            foreach (Type? type in validatorTypes)
            {
                Type interfaceType = validatorType.MakeGenericType(GetValidatedType(type, validatorType));
                services.AddScoped(interfaceType, type);
            }

            return services;
        }

        private static bool IsValidator(Type type, Type validatorType)
        {
            while (type != null && type != typeof(object))
            {
                Type current = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (validatorType == current)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        private static Type GetValidatedType(Type type, Type validatorType)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == validatorType)
                {
                    return type.GetGenericArguments()[0];
                }

                type = type.BaseType;
            }

            throw new ArgumentException($"Could not find validated type for validator {type}");
        }
    }
}