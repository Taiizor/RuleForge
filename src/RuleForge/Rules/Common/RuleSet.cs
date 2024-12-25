namespace RuleForge.Rules.Common
{
    public class RuleSet<T>
    {
        private readonly List<RuleBuilder<T, object>> _rules;
        private CascadeMode _cascadeMode;
        private AsyncCascadeMode _asyncCascadeMode;

        public RuleSet(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _rules = new List<RuleBuilder<T, object>>();
            _cascadeMode = CascadeMode.Continue;
            _asyncCascadeMode = AsyncCascadeMode.Continue;
        }

        public string Name { get; }

        public void SetCascadeMode(CascadeMode mode)
        {
            _cascadeMode = mode;
        }

        public void SetAsyncCascadeMode(AsyncCascadeMode mode)
        {
            _asyncCascadeMode = mode;
        }

        public void AddRule(RuleBuilder<T, object> rule)
        {
            _rules.Add(rule ?? throw new ArgumentNullException(nameof(rule)));
        }

        public ValidationResult Validate(T instance)
        {
            List<ValidationError> errors = new();

            foreach (RuleBuilder<T, object> rule in _rules)
            {
                ValidationResult result = rule.Validate(instance);
                if (!result.IsValid)
                {
                    errors.AddRange(result.Errors);
                    if (_cascadeMode == CascadeMode.StopOnFirstFailure)
                    {
                        break;
                    }
                }
            }

            return new ValidationResult(errors);
        }

        public async Task<ValidationResult> ValidateAsync(T instance)
        {
            List<ValidationError> errors = new();

            foreach (RuleBuilder<T, object> rule in _rules)
            {
                ValidationResult result = await rule.ValidateAsync(instance);
                if (!result.IsValid)
                {
                    errors.AddRange(result.Errors);
                    if (_asyncCascadeMode == AsyncCascadeMode.StopOnFirstFailure)
                    {
                        break;
                    }
                }
            }

            return new ValidationResult(errors);
        }
    }
}