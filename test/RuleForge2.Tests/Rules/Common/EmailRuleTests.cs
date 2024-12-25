using Xunit;
using RuleForge2.Core.Rules.Common;

namespace RuleForge2.Tests.Rules.Common
{
    public class EmailRuleTests
    {
        [Theory]
        [InlineData("test@example.com", true)]
        [InlineData("test.name@example.com", true)]
        [InlineData("test+name@example.com", true)]
        [InlineData("test@sub.example.com", true)]
        [InlineData("test", false)]
        [InlineData("test@", false)]
        [InlineData("@example.com", false)]
        [InlineData("test@example", false)]
        [InlineData("", false)]
        [InlineData(null, true)] // Null is handled by NotEmptyRule
        public void Validate_EmailAddress_ReturnsExpectedResult(string email, bool expectedIsValid)
        {
            // Arrange
            var rule = new EmailRule("Email");

            // Act
            var result = rule.Validate(email);

            // Assert
            Assert.Equal(expectedIsValid, result.IsValid);
            if (!expectedIsValid)
            {
                Assert.Single(result.Errors);
                Assert.Equal("Email must be a valid email address", result.Errors[0].ErrorMessage);
            }
        }

        [Fact]
        public void Validate_WithCustomErrorMessage_ReturnsCustomMessage()
        {
            // Arrange
            var rule = new EmailRule("Email") { ErrorMessage = "Custom error message" };
            var email = "invalid-email";

            // Act
            var result = rule.Validate(email);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal("Custom error message", result.Errors[0].ErrorMessage);
        }
    }
}