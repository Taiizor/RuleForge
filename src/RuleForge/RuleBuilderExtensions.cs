using RuleForge.Rules;
using RuleForge.Rules.Common;
using System.Text.RegularExpressions;

namespace RuleForge
{
    public static class RuleBuilderExtensions
    {
        public static RuleBuilder<T, string> NotEmpty<T>(this RuleBuilder<T, string> builder, string message = null)
        {
            return builder.AddRule(new NotEmptyRule<string>(message));
        }

        public static RuleBuilder<T, string> Length<T>(this RuleBuilder<T, string> builder, int min, int max, string message = null)
        {
            return builder.AddRule(new LengthRule(min, max, message));
        }

        public static RuleBuilder<T, string> MinLength<T>(this RuleBuilder<T, string> builder, int min, string message = null)
        {
            return builder.AddRule(new LengthRule(min, int.MaxValue, message ?? $"Minimum length is {min}"));
        }

        public static RuleBuilder<T, string> MaxLength<T>(this RuleBuilder<T, string> builder, int max, string message = null)
        {
            return builder.AddRule(new LengthRule(0, max, message ?? $"Maximum length is {max}"));
        }

        public static RuleBuilder<T, string> EmailAddress<T>(this RuleBuilder<T, string> builder, string message = null)
        {
            return builder.AddRule(new EmailRule(message));
        }

        public static RuleBuilder<T, TProperty> GreaterThan<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            TProperty value,
            string message = null) where TProperty : IComparable<TProperty>
        {
            return builder.AddRule(new ComparisonRule<TProperty>(value, ComparisonType.GreaterThan, message));
        }

        public static RuleBuilder<T, TProperty> GreaterThanOrEqual<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            TProperty value,
            string message = null) where TProperty : IComparable<TProperty>
        {
            return builder.AddRule(new ComparisonRule<TProperty>(value, ComparisonType.GreaterThanOrEqual, message));
        }

        public static RuleBuilder<T, TProperty> LessThan<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            TProperty value,
            string message = null) where TProperty : IComparable<TProperty>
        {
            return builder.AddRule(new ComparisonRule<TProperty>(value, ComparisonType.LessThan, message));
        }

        public static RuleBuilder<T, TProperty> LessThanOrEqual<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            TProperty value,
            string message = null) where TProperty : IComparable<TProperty>
        {
            return builder.AddRule(new ComparisonRule<TProperty>(value, ComparisonType.LessThanOrEqual, message));
        }

        public static RuleBuilder<T, TProperty> Equal<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            TProperty value,
            string message = null) where TProperty : IComparable<TProperty>
        {
            return builder.AddRule(new ComparisonRule<TProperty>(value, ComparisonType.Equal, message));
        }

        public static RuleBuilder<T, TProperty> NotEqual<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            TProperty value,
            string message = null) where TProperty : IComparable<TProperty>
        {
            return builder.AddRule(new ComparisonRule<TProperty>(value, ComparisonType.NotEqual, message));
        }

        public static RuleBuilder<T, string> Matches<T>(
            this RuleBuilder<T, string> builder,
            string pattern,
            RegexOptions options = RegexOptions.None,
            string? message = null)
        {
            return builder.AddRule(new RegexRule(pattern, options, message));
        }

        public static RuleBuilder<T, string> CreditCard<T>(
            this RuleBuilder<T, string> builder,
            string? message = null)
        {
            return builder.AddRule(new CreditCardRule(message));
        }

        public static RuleBuilder<T, TProperty> ExclusiveBetween<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            TProperty from,
            TProperty to,
            string? message = null) where TProperty : IComparable<TProperty>
        {
            return builder.AddRule(new ExclusiveBetweenRule<TProperty>(from, to, message));
        }

        public static RuleBuilder<T, TProperty> InclusiveBetween<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            TProperty from,
            TProperty to,
            string? message = null) where TProperty : IComparable<TProperty>
        {
            return builder.AddRule(new InclusiveBetweenRule<TProperty>(from, to, message));
        }

        public static RuleBuilder<T, decimal> ScalePrecision<T>(
            this RuleBuilder<T, decimal> builder,
            int precision,
            int scale,
            string? message = null)
        {
            return builder.AddRule(new ScalePrecisionRule(precision, scale, message));
        }

        public static RuleBuilder<T, string> PhoneNumber<T>(
            this RuleBuilder<T, string> builder,
            string? message = null)
        {
            return builder.AddRule(new PhoneNumberRule(message));
        }

        public static RuleBuilder<T, string> Url<T>(
            this RuleBuilder<T, string> builder,
            bool requireHttps = false,
            string? message = null)
        {
            return builder.AddRule(new UrlRule(requireHttps, message));
        }

        public static RuleBuilder<T, TEnum> IsInEnum<T, TEnum>(
            this RuleBuilder<T, TEnum> builder,
            string? message = null) where TEnum : struct, Enum
        {
            return builder.AddRule(new EnumRule<TEnum>(message));
        }

        public static RuleBuilder<T, DateTime> Future<T>(
            this RuleBuilder<T, DateTime> builder,
            string? message = null)
        {
            return builder.AddRule(new DateTimeRule(DateTimeRuleType.Future, errorMessage: message));
        }

        public static RuleBuilder<T, DateTime> FutureOrPresent<T>(
            this RuleBuilder<T, DateTime> builder,
            string? message = null)
        {
            return builder.AddRule(new DateTimeRule(DateTimeRuleType.FutureOrPresent, errorMessage: message));
        }

        public static RuleBuilder<T, DateTime> Past<T>(
            this RuleBuilder<T, DateTime> builder,
            string? message = null)
        {
            return builder.AddRule(new DateTimeRule(DateTimeRuleType.Past, errorMessage: message));
        }

        public static RuleBuilder<T, DateTime> PastOrPresent<T>(
            this RuleBuilder<T, DateTime> builder,
            string? message = null)
        {
            return builder.AddRule(new DateTimeRule(DateTimeRuleType.PastOrPresent, errorMessage: message));
        }

        public static RuleBuilder<T, DateTime> After<T>(
            this RuleBuilder<T, DateTime> builder,
            DateTime date,
            string? message = null)
        {
            return builder.AddRule(new DateTimeRule(DateTimeRuleType.After, date, message));
        }

        public static RuleBuilder<T, DateTime> Before<T>(
            this RuleBuilder<T, DateTime> builder,
            DateTime date,
            string? message = null)
        {
            return builder.AddRule(new DateTimeRule(DateTimeRuleType.Before, date, message));
        }

        public static RuleBuilder<T, IEnumerable<TElement>> MinimumLength<T, TElement>(
            this RuleBuilder<T, IEnumerable<TElement>> builder,
            int minimumLength,
            string? message = null)
        {
            return builder.AddRule(new ListRule<TElement>(ListRuleType.MinimumLength, minimumLength, message));
        }

        public static RuleBuilder<T, IEnumerable<TElement>> MaximumLength<T, TElement>(
            this RuleBuilder<T, IEnumerable<TElement>> builder,
            int maximumLength,
            string? message = null)
        {
            return builder.AddRule(new ListRule<TElement>(ListRuleType.MaximumLength, maximumLength, message));
        }

        public static RuleBuilder<T, IEnumerable<TElement>> ExactLength<T, TElement>(
            this RuleBuilder<T, IEnumerable<TElement>> builder,
            int exactLength,
            string? message = null)
        {
            return builder.AddRule(new ListRule<TElement>(ListRuleType.ExactLength, exactLength, message));
        }

        public static RuleBuilder<T, IEnumerable<TElement>> Unique<T, TElement>(
            this RuleBuilder<T, IEnumerable<TElement>> builder,
            string? message = null)
        {
            return builder.AddRule(new ListRule<TElement>(ListRuleType.Unique, errorMessage: message));
        }

        public static RuleBuilder<T, Guid> Guid<T>(
            this RuleBuilder<T, Guid> builder,
            bool allowEmpty = false,
            string? message = null)
        {
            return builder.AddRule(new GuidRule(allowEmpty, message));
        }

        public static RuleBuilder<T, string> IPAddress<T>(
            this RuleBuilder<T, string> builder,
            bool allowIPv4 = true,
            bool allowIPv6 = true,
            string? message = null)
        {
            return builder.AddRule(new IPAddressRule(allowIPv4, allowIPv6, message));
        }

        public static RuleBuilder<T, string> Password<T>(
            this RuleBuilder<T, string> builder,
            Action<PasswordOptions>? configure = null,
            string? message = null)
        {
            var options = new PasswordOptions();
            configure?.Invoke(options);
            return builder.AddRule(new PasswordRule(options, message));
        }

        public static RuleBuilder<T, string> Json<T>(
            this RuleBuilder<T, string> builder,
            Action<JsonSchemaOptions>? configure = null,
            string? message = null)
        {
            var options = new JsonSchemaOptions();
            configure?.Invoke(options);
            return builder.AddRule(new JsonRule(options, message));
        }

        public static RuleBuilder<T, string> AllowedExtensions<T>(
            this RuleBuilder<T, string> builder,
            IEnumerable<string> allowedExtensions,
            bool caseSensitive = false,
            string? message = null)
        {
            return builder.AddRule(new FileExtensionRule(allowedExtensions, caseSensitive, message));
        }

        public static RuleBuilder<T, TValue> Range<T, TValue>(
            this RuleBuilder<T, TValue> builder,
            TValue? minimum = default,
            TValue? maximum = default,
            bool inclusive = true,
            string? message = null) where TValue : IComparable<TValue>
        {
            return builder.AddRule(new NumericRangeRule<TValue>(minimum, maximum, inclusive, message));
        }

        public static RuleBuilder<T, string> Color<T>(
            this RuleBuilder<T, string> builder,
            ColorFormat allowedFormats = ColorFormat.All,
            string? message = null)
        {
            return builder.AddRule(new ColorRule(allowedFormats, message));
        }

        public static RuleBuilder<T, TimeSpan> TimeSpan<T>(
            this RuleBuilder<T, TimeSpan> builder,
            TimeSpan? minimum = null,
            TimeSpan? maximum = null,
            bool allowZero = true,
            bool allowNegative = false,
            string? message = null)
        {
            return builder.AddRule(new TimeSpanRule(minimum, maximum, allowZero, allowNegative, message));
        }

        public static RuleBuilder<T, TValue> Composite<T, TValue>(
            this RuleBuilder<T, TValue> builder,
            IEnumerable<IRule<TValue>> rules,
            CompositeRuleOperator @operator = CompositeRuleOperator.And,
            string? message = null)
        {
            return builder.AddRule(new CompositeRule<TValue>(rules, @operator, message));
        }

        public static RuleBuilder<T, TProperty> Custom<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            T instance,
            Func<T, TProperty?, ValidationResult> validator,
            string? message = null)
        {
            return builder.AddRule(new CustomValidatorRule<T, TProperty>(instance, validator, message));
        }

        public static RuleBuilder<T, TProperty> CustomAsync<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            T instance,
            Func<T, TProperty?, Task<ValidationResult>> asyncValidator,
            string? message = null)
        {
            return builder.AddRule(new CustomValidatorRule<T, TProperty>(instance, asyncValidator, message));
        }

        public static RuleBuilder<T, TProperty> DependentOn<T, TProperty, TDependency>(
            this RuleBuilder<T, TProperty> builder,
            T instance,
            Func<T, TDependency> dependencySelector,
            Func<TProperty?, TDependency, ValidationResult> validator,
            string? message = null)
        {
            return builder.AddRule(new DependentRule<T, TProperty, TDependency>(
                instance, dependencySelector, validator, message));
        }

        public static RuleBuilder<T, TProperty> When<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            T instance,
            Func<T, bool> condition,
            IRule<TProperty> rule,
            string? message = null)
        {
            return builder.AddRule(new WhenRule<T, TProperty>(instance, condition, rule, message));
        }

        public static RuleBuilder<T, TProperty> Unless<T, TProperty>(
            this RuleBuilder<T, TProperty> builder,
            T instance,
            Func<T, bool> condition,
            IRule<TProperty> rule,
            string? message = null)
        {
            return builder.AddRule(new UnlessRule<T, TProperty>(instance, condition, rule, message));
        }
    }
}