using System;
using System.Threading.Tasks;

namespace RuleForge.Rules.Common
{
    public class ChildValidatorRule<TParent, TChild> : IRule<TChild>
    {
        private readonly Validator<TChild> _validator;
        private readonly ValidationContext<TParent> _parentContext;

        public ChildValidatorRule(Validator<TChild> validator, ValidationContext<TParent> parentContext = null)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _parentContext = parentContext;
        }

        public ValidationResult Validate(TChild value)
        {
            if (value == null)
            {
                return ValidationResult.Success();
            }

            var context = new ValidationContext<TChild>(value, null, value, _parentContext?.RootInstance, _parentContext as ValidationContext<TChild>);
            return _validator.Validate(context);
        }

        public async Task<ValidationResult> ValidateAsync(TChild value)
        {
            if (value == null)
            {
                return ValidationResult.Success();
            }

            var context = new ValidationContext<TChild>(value, null, value, _parentContext?.RootInstance, _parentContext as ValidationContext<TChild>);
            return await _validator.ValidateAsync(context);
        }
    }
}