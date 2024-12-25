namespace RuleForge.Rules.Collections
{
    public class CollectionRule<T> : IRule<IEnumerable<T>>
    {
        private readonly IRule<T> _itemRule;
        public string ErrorMessage { get; }

        public CollectionRule(IRule<T> itemRule, string errorMessage = null)
        {
            _itemRule = itemRule;
            ErrorMessage = errorMessage;
        }

        public ValidationResult Validate(IEnumerable<T> value)
        {
            if (value == null)
            {
                return ValidationResult.Success();
            }

            List<ValidationError> errors = new();
            int index = 0;

            foreach (T? item in value)
            {
                ValidationResult result = _itemRule.Validate(item);
                if (!result.IsValid)
                {
                    foreach (ValidationError error in result.Errors)
                    {
                        errors.Add(new ValidationError(
                            $"[{index}].{error.PropertyName}",
                            error.ErrorMessage));
                    }
                }
                index++;
            }

            return new ValidationResult(errors);
        }

        public async Task<ValidationResult> ValidateAsync(IEnumerable<T> value)
        {
            if (value == null)
            {
                return ValidationResult.Success();
            }

            List<ValidationError> errors = new();
            int index = 0;

            foreach (T? item in value)
            {
                ValidationResult result = await _itemRule.ValidateAsync(item);
                if (!result.IsValid)
                {
                    foreach (ValidationError error in result.Errors)
                    {
                        errors.Add(new ValidationError(
                            $"[{index}].{error.PropertyName}",
                            error.ErrorMessage));
                    }
                }
                index++;
            }

            return new ValidationResult(errors);
        }
    }
}