using FluentAssertions;
using ITG.Brix.Users.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ITG.Brix.Users.UnitTests.Domain
{
    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public void LoginShouldHaveNoPropertiesWithPublicSetter()
        {
            var propertiesWithPublicSetterCount = typeof(Login).GetProperties().Count(x => x.GetSetMethod() != null);
            propertiesWithPublicSetterCount.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow("Login")]
        public void CreateLoginShouldSucceed(string value)
        {
            // Act
            var result = new Login(value);

            // Assert
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void CreateLoginShouldFailWhenValueIsNull()
        {
            // Arrange
            string value = null;

            // Act
            Action ctor = () => { new Login(value); };

            // Assert
            ctor.Should().Throw<ArgumentException>().WithMessage($"*{nameof(value)}*");
        }

        [TestMethod]
        public void CreateLoginShouldFailWhenValueIsEmptyString()
        {
            // Arrange
            var value = string.Empty;

            // Act
            Action ctor = () => { new Login(value); };

            // Assert
            ctor.Should().Throw<ArgumentException>().WithMessage($"*{nameof(value)}*");
        }

        [TestMethod]
        public void CreateLoginShouldFailWhenValueIsWhiteSpace()
        {
            // Arrange
            string value = "   ";

            // Act
            Action ctor = () => { new Login(value); };

            // Assert
            ctor.Should().Throw<ArgumentException>().WithMessage($"*{nameof(value)}*");
        }

        [TestMethod]
        public void TwoLoginWithSameValueShouldBeEqualThroughMethod()
        {
            // Arrange
            var value = "BrixUserLogin";

            var first = new Login(value);
            var second = new Login(value);

            // Act
            var result = first.Equals(second);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void TwoLoginWithSameValueShouldBeEqualThroughOperator()
        {
            // Arrange
            var value = "BrixUserLogin";

            var first = new Login(value);
            var second = new Login(value);

            // Act
            var result = first == second;

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void TwoLoginWithDistinctValueShouldBeNotEqualThroughMethod()
        {
            // Arrange
            var value1 = "BrixUserLogin-1";
            var value2 = "BrixUserLogin-2";
            var first = new Login(value1);
            var second = new Login(value2);

            // Act
            var result = first.Equals(second);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void TwoSourcesWithDistinctValuesShouldBeNotEqualThroughOperator()
        {
            // Arrange
            var value1 = "BrixUserLogin-1";
            var value2 = "BrixUserLogin-2";
            var first = new Login(value1);
            var second = new Login(value2);

            // Act
            var result = first != second;

            // Assert
            result.Should().BeTrue();
        }
    }
}
