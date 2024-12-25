using Xunit;
using RuleForge2.Core.Rules.Collections;
using RuleForge2.Core.Rules.Common;
using System.Collections.Generic;

namespace RuleForge2.Tests.Rules.Collections
{
    public class CollectionRuleTests
    {
        [Fact]
        public void Validate_ValidCollection_ReturnsSuccess()
        {
            // Arrange
            var itemRule = new NotEmptyRule<string>("Item");
            var rule = new CollectionRule<string>("Items", itemRule);
            var collection = new List<string> { "Item1", "Item2", "Item3" };

            // Act
            var result = rule.Validate(collection);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Validate_CollectionWithInvalidItems_ReturnsErrors()
        {
            // Arrange
            var itemRule = new NotEmptyRule<string>("Item");
            var rule = new CollectionRule<string>("Items", itemRule);
            var collection = new List<string> { "Item1", "", "Item3", "   " };

            // Act
            var result = rule.Validate(collection);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(2, result.Errors.Count);
        }

        [Fact]
        public void Validate_NullCollection_ReturnsSuccess()
        {
            // Arrange
            var itemRule = new NotEmptyRule<string>("Item");
            var rule = new CollectionRule<string>("Items", itemRule);
            List<string> collection = null;

            // Act
            var result = rule.Validate(collection);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Validate_EmptyCollection_ReturnsSuccess()
        {
            // Arrange
            var itemRule = new NotEmptyRule<string>("Item");
            var rule = new CollectionRule<string>("Items", itemRule);
            var collection = new List<string>();

            // Act
            var result = rule.Validate(collection);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}