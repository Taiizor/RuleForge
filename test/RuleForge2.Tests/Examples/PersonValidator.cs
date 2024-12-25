using RuleForge2.Core;
using RuleForge2.Extensions;

namespace RuleForge2.Tests.Examples
{
    public class PersonValidator : Validator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(2, 50);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(2, 50);

            RuleFor(x => x.Email)
                .NotEmpty()
                .Email();

            RuleFor(x => x.Address)
                .NotEmpty()
                .SetValidator(new AddressValidator());
        }
    }
}