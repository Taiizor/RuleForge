namespace RuleForge.Rules.Common
{
    public class DateTimeRule : IRule<DateTime>
    {
        private readonly DateTimeRuleType _ruleType;
        private readonly DateTime? _comparisonDate;
        public string ErrorMessage { get; }

        public DateTimeRule(DateTimeRuleType ruleType, DateTime? comparisonDate = null, string? errorMessage = null)
        {
            _ruleType = ruleType;
            _comparisonDate = comparisonDate;
            ErrorMessage = errorMessage ?? GetDefaultErrorMessage();
        }

        public ValidationResult Validate(DateTime value)
        {
            bool isValid = _ruleType switch
            {
                DateTimeRuleType.Future => value > DateTime.Now,
                DateTimeRuleType.FutureOrPresent => value >= DateTime.Now,
                DateTimeRuleType.Past => value < DateTime.Now,
                DateTimeRuleType.PastOrPresent => value <= DateTime.Now,
                DateTimeRuleType.After => _comparisonDate.HasValue && value > _comparisonDate,
                DateTimeRuleType.AfterOrEqual => _comparisonDate.HasValue && value >= _comparisonDate,
                DateTimeRuleType.Before => _comparisonDate.HasValue && value < _comparisonDate,
                DateTimeRuleType.BeforeOrEqual => _comparisonDate.HasValue && value <= _comparisonDate,
                _ => throw new ArgumentException("Invalid rule type")
            };

            return isValid
                ? ValidationResult.Success()
                : ValidationResult.Error("DateTime", ErrorMessage);
        }

        public Task<ValidationResult> ValidateAsync(DateTime value)
        {
            return Task.FromResult(Validate(value));
        }

        private string GetDefaultErrorMessage()
        {
            return _ruleType switch
            {
                DateTimeRuleType.Future => "Date must be in the future",
                DateTimeRuleType.FutureOrPresent => "Date must be in the future or present",
                DateTimeRuleType.Past => "Date must be in the past",
                DateTimeRuleType.PastOrPresent => "Date must be in the past or present",
                DateTimeRuleType.After => $"Date must be after {_comparisonDate}",
                DateTimeRuleType.AfterOrEqual => $"Date must be after or equal to {_comparisonDate}",
                DateTimeRuleType.Before => $"Date must be before {_comparisonDate}",
                DateTimeRuleType.BeforeOrEqual => $"Date must be before or equal to {_comparisonDate}",
                _ => "Invalid date"
            };
        }
    }

    public enum DateTimeRuleType
    {
        Future,
        FutureOrPresent,
        Past,
        PastOrPresent,
        After,
        AfterOrEqual,
        Before,
        BeforeOrEqual
    }
}