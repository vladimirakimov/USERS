using FluentAssertions;
using ITG.Brix.Users.Application.DataTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.Application.DataTypes
{
    [TestClass]
    public class OptionalTests
    {
        [TestMethod]
        public void ParametlessConstructorShouldCreateOptionalWithoutValue()
        {
            // Act
            var optional = new Optional<int>();

            // Assert
            optional.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void ConstructorWithParametersShouldCreateOptionalWithValue()
        {
            // Arrange
            var value = 4;

            // Act
            var optional = new Optional<int>(value);

            // Assert
            optional.HasValue.Should().BeTrue();
        }

        [TestMethod]
        public void AccessValueOfOptionalWithoutValueShouldFail()
        {
            // Arrange
            var optional = new Optional<int>();

            // Act
            Action action = () => { var temp = optional.Value; };

            // Assert
            action.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void SameTypeOptionalsWithSameValueShouldBeEqual()
        {
            // Arrange
            var value = 4;
            var optional1 = new Optional<int>(value);
            var optional2 = new Optional<int>(value);

            // Act
            var result = optional1.Equals(optional2);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void SameTypeOptionalWithoutValueShouldBeEqual()
        {
            // Arrange
            var optional1 = new Optional<string>();
            var optional2 = new Optional<string>();

            // Act
            var result = optional1.Equals(optional2);

            // Assert
            result.Should().BeTrue();
        }
    }
}
