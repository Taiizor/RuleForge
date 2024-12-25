using Xunit;
using RuleForge2.Core.Validation;

namespace RuleForge2.Tests.Core
{
    public class ValidationResultTests
    {
        [Fact]
        public void Success_ReturnsValidResult()
        {
            // Act
            var result = ValidationResult.Success();

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Error_ReturnsInvalidResult()
        {
            // Act
            var result = ValidationResult.Error("TestProperty", "Error message");

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal("TestProperty", result.Errors[0].PropertyName);
            Assert.Equal("Error message", result.Errors[0].ErrorMessage);
            Assert.Equal(Severity.Error, result.Errors[0].Severity);
        }

        [Fact]
        public void Combine_WithNoResults_ReturnsSuccess()
        {
            // Act
            var result = ValidationResult.Combine();

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Combine_WithValidResults_ReturnsSuccess()
        {
            // Arrange
            var results = new[]
            {
                ValidationResult.Success(),
                ValidationResult.Success()
            };

            // Act
            var result = ValidationResult.Combine(results);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Combine_WithInvalidResults_ReturnsCombinedErrors()
        {
            // Arrange
            var results = new[]
            {
                ValidationResult.Error("Property1", "Error 1"),
                ValidationResult.Success(),
                ValidationResult.Error("Property2", "Error 2")
            };

            // Act
            var result = ValidationResult.Combine(results);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains(result.Errors, e => e.PropertyName == "Property1" && e.ErrorMessage == "Error 1");
            Assert.Contains(result.Errors, e => e.PropertyName == "Property2" && e.ErrorMessage == "Error 2");
        }

        [Fact]
        public void AddError_AddsErrorToResult()
        {
            // Arrange
            var result = new ValidationResult();

            // Act
            result.AddError("TestProperty", "Error message", Severity.Warning);

            // Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Equal("TestProperty", result.Errors[0].PropertyName);
            Assert.Equal("Error message", result.Errors[0].ErrorMessage);
            Assert.Equal(Severity.Warning, result.Errors[0].Severity);
        }
    }
}