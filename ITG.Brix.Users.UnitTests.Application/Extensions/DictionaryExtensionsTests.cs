using FluentAssertions;
using ITG.Brix.Users.Application.DataTypes;
using ITG.Brix.Users.Application.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ITG.Brix.Users.UnitTests.Application.Extensions
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        [TestMethod]
        public void GetOptionalShouldReturnWithValue()
        {
            // Arrange
            var valuePairsDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Key", "Value" }
            };

            // Act
            Optional<string> result = valuePairsDictionary.GetOptional("key");

            // Assert
            result.HasValue.Should().BeTrue();
        }

        [TestMethod]
        public void GetOptionalShouldReturnWithoutValue()
        {
            // Arrange
            var valuePairsDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Key", "Value" }
            };

            // Act
            Optional<string> result = valuePairsDictionary.GetOptional("unexistingKey");

            // Assert
            result.HasValue.Should().BeFalse();
        }
    }
}
