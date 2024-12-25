using Xunit;
using RuleForge.Core.Rules.Common;

namespace RuleForge.Tests.Rules.Common
{
    public class NotEmptyRuleTests
    {
        [Fact]
        public void Validate_WhenValueIsNull_ReturnsError()
        {
            // Arrange
            var rule = new NotEmptyRule<string>("TestProperty");
            string value = null;

            // Act
            var result = rule.Validate(value);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal("TestProperty cannot be empty", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Validate_WhenValueIsEmpty_ReturnsError()
        {
            // Arrange
            var rule = new NotEmptyRule<string>("TestProperty");
            var value = string.Empty;

            // Act
            var result = rule.Validate(value);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal("TestProperty cannot be empty", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Validate_WhenValueIsWhitespace_ReturnsError()
        {
            // Arrange
            var rule = new NotEmptyRule<string>("TestProperty");
            var value = "   ";

            // Act
            var result = rule.Validate(value);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal("TestProperty cannot be empty", result.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Validate_WhenValueIsNotEmpty_ReturnsSuccess()
        {
            // Arrange
            var rule = new NotEmptyRule<string>("TestProperty");
            var value = "Test Value";

            // Act
            var result = rule.Validate(value);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}