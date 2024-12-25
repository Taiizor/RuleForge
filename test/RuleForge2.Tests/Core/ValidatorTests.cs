using Xunit;
using RuleForge2.Tests.Examples;

namespace RuleForge2.Tests.Core
{
    public class ValidatorTests
    {
        [Fact]
        public void Validate_ValidPerson_ReturnsSuccess()
        {
            // Arrange
            var validator = new PersonValidator();
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "New York",
                    Country = "USA"
                }
            };

            // Act
            var result = validator.Validate(person);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Validate_InvalidPerson_ReturnsErrors()
        {
            // Arrange
            var validator = new PersonValidator();
            var person = new Person
            {
                FirstName = "",
                LastName = "D",
                Email = "invalid-email",
                Address = new Address
                {
                    Street = "",
                    City = "A",
                    Country = ""
                }
            };

            // Act
            var result = validator.Validate(person);

            // Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count > 0);
        }

        [Fact]
        public async Task ValidateAsync_ValidPerson_ReturnsSuccess()
        {
            // Arrange
            var validator = new PersonValidator();
            var person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "New York",
                    Country = "USA"
                }
            };

            // Act
            var result = await validator.ValidateAsync(person);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task ValidateAsync_InvalidPerson_ReturnsErrors()
        {
            // Arrange
            var validator = new PersonValidator();
            var person = new Person
            {
                FirstName = "",
                LastName = "D",
                Email = "invalid-email",
                Address = new Address
                {
                    Street = "",
                    City = "A",
                    Country = ""
                }
            };

            // Act
            var result = await validator.ValidateAsync(person);

            // Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Count > 0);
        }
    }
}