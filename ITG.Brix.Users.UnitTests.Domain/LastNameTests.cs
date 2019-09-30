using FluentAssertions;
using ITG.Brix.Users.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ITG.Brix.Users.UnitTests.Domain
{
    [TestClass]
    public class LastNameTests
    {
        [TestMethod]
        public void ShouldHaveNoPropertiesWithPublicSetter()
        {
            var propertiesWithPublicSetterCount = typeof(LastName).GetProperties().Count(x => x.GetSetMethod() != null);
            propertiesWithPublicSetterCount.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow("LastName")]
        [DataRow("Last Name")]
        public void CreateLastNameShouldSucceed(string value)
        {
            // Act
            var result = new LastName(value);

            // Assert
            result.Value.Should().Be(value);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public void CreateLastNameShouldFailWhenPasswordIsNullOrIsEmptyOrIsWhiteSpace(string value)
        {
            // Act
            Action ctor = () => { new LastName(value); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>().WithMessage($"*{nameof(value)}*");
        }
    }
}
