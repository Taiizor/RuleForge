namespace RuleForge.Rules.Common
{
    public class ListRule<T> : IRule<IEnumerable<T>>
    {
        private readonly ListRuleType _ruleType;
        private readonly int? _count;
        public string ErrorMessage { get; }

        public ListRule(ListRuleType ruleType, int? count = null, string? errorMessage = null)
        {
            _ruleType = ruleType;
            _count = count;
            ErrorMessage = errorMessage ?? GetDefaultErrorMessage();
        }

        public ValidationResult Validate(IEnumerable<T>? value)
        {
            if (value == null)
            {
                return ValidationResult.Success();
            }

            int itemCount = value.Count();
            bool isValid = _ruleType switch
            {
                ListRuleType.NotEmpty => itemCount > 0,
                ListRuleType.MinimumLength => _count.HasValue && itemCount >= _count.Value,
                ListRuleType.MaximumLength => _count.HasValue && itemCount <= _count.Value,
                ListRuleType.ExactLength => _count.HasValue && itemCount == _count.Value,
                ListRuleType.Unique => value.Distinct().Count() == itemCount,
                _ => throw new ArgumentException("Invalid rule type")
            };

            return isValid 
                ? ValidationResult.Success() 
                : ValidationResult.Error("List", ErrorMessage);
        }

        public Task<ValidationResult> ValidateAsync(IEnumerable<T>? value)
        {
            return Task.FromResult(Validate(value));
        }

        private string GetDefaultErrorMessage()
        {
            return _ruleType switch
            {
                ListRuleType.NotEmpty => "List cannot be empty",
                ListRuleType.MinimumLength => $"List must contain at least {_count} items",
                ListRuleType.MaximumLength => $"List cannot contain more than {_count} items",
                ListRuleType.ExactLength => $"List must contain exactly {_count} items",
                ListRuleType.Unique => "All items in the list must be unique",
                _ => "Invalid list"
            };
        }
    }

    public enum ListRuleType
    {
        NotEmpty,
        MinimumLength,
        MaximumLength,
        ExactLength,
        Unique
    }
}