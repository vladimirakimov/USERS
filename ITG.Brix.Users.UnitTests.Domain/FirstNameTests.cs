using FluentAssertions;
using ITG.Brix.Users.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ITG.Brix.Users.UnitTests.Domain
{
    [TestClass]
    public class FirstNameTests
    {
        [TestMethod]
        public void ShouldHaveNoPropertiesWithPublicSetter()
        {
            var propertiesWithPublicSetterCount = typeof(FirstName).GetProperties().Count(x => x.GetSetMethod() != null);
            propertiesWithPublicSetterCount.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow("FirstName")]
        [DataRow("First Name")]
        public void CreateFirstNameShouldSucceed(string value)
        {
            // Act
            var result = new FirstName(value);

            // Assert
            result.Value.Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void CreateFirstNameShouldFailWhenPasswordIsNullOrIsEmptyOrIsWhiteSpace(string value)
        {
            // Act
            Action ctor = () => { new FirstName(value); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>().WithMessage($"*{nameof(value)}*");
        }
    }
}
