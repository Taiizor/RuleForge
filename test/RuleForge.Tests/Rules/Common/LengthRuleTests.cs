using Xunit;
using RuleForge.Core.Rules.Common;

namespace RuleForge.Tests.Rules.Common
{
    public class LengthRuleTests
    {
        [Theory]
        [InlineData("abc", 2, 5, true)]
        [InlineData("abcdef", 2, 5, false)]
        [InlineData("a", 2, 5, false)]
        [InlineData("", 2, 5, false)]
        [InlineData(null, 2, 5, true)] // Null is handled by NotEmptyRule
        public void Validate_StringLength_ReturnsExpectedResult(string value, int minLength, int maxLength, bool expectedIsValid)
        {
            // Arrange
            var rule = new LengthRule("TestProperty", minLength, maxLength);

            // Act
            var result = rule.Validate(value);

            // Assert
            Assert.Equal(expectedIsValid, result.IsValid);
            if (!expectedIsValid)
            {
                Assert.Single(result.Errors);
                Assert.Equal($"TestProperty must be between {minLength} and {maxLength} characters", result.Errors[0].ErrorMessage);
            }
        }

        [Fact]
        public void Validate_WithCustomErrorMessage_ReturnsCustomMessage()
        {
            // Arrange
            var rule = new LengthRule("TestProperty", 2, 5) { ErrorMessage = "Custom error message" };
            var value = "a";

            // Act
            var result = rule.Validate(value);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal("Custom error message", result.Errors[0].ErrorMessage);
        }
    }
}