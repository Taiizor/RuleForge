using RuleForge2.Core;
using RuleForge2.Extensions;

namespace RuleForge2.Tests.Examples
{
    public class AddressValidator : Validator<Address>
    {
        public AddressValidator()
        {
            RuleFor(x => x.Street)
                .NotEmpty();

            RuleFor(x => x.Street)
                .When(x => !string.IsNullOrWhiteSpace(x.Street))
                .Length(5, 100);

            RuleFor(x => x.City)
                .NotEmpty();

            RuleFor(x => x.City)
                .When(x => !string.IsNullOrWhiteSpace(x.City))
                .Length(2, 50);

            RuleFor(x => x.Country)
                .NotEmpty();

            RuleFor(x => x.Country)
                .When(x => !string.IsNullOrWhiteSpace(x.Country))
                .Length(2, 50);
        }
    }
}