using Xunit;
using RuleForge.Core.Rules.Common;
using RuleForge.Tests.Examples;

namespace RuleForge.Tests.Rules.Common
{
    public class ChildValidatorRuleTests
    {
        [Fact]
        public void Validate_ValidAddress_ReturnsSuccess()
        {
            // Arrange
            var rule = new ChildValidatorRule<Address>("Address", new AddressValidator());
            var address = new Address
            {
                Street = "123 Main St",
                City = "New York",
                Country = "USA"
            };

            // Act
            var result = rule.Validate(address);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Validate_InvalidAddress_ReturnsErrors()
        {
            // Arrange
            var rule = new ChildValidatorRule<Address>("Address", new AddressValidator());
            var address = new Address
            {
                Street = "",
                City = "A",
                Country = ""
            };

            // Act
            var result = rule.Validate(address);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(3, result.Errors.Count);
        }

        [Fact]
        public void Validate_NullAddress_ReturnsSuccess()
        {
            // Arrange
            var rule = new ChildValidatorRule<Address>("Address", new AddressValidator());
            Address address = null;

            // Act
            var result = rule.Validate(address);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}