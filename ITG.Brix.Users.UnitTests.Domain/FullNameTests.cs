using FluentAssertions;
using ITG.Brix.Users.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ITG.Brix.Users.UnitTests.Domain
{
    [TestClass]
    public class FullNameTests
    {
        [TestMethod]
        public void ShouldHaveNoPropertiesWithPublicSetter()
        {
            var propertiesWithPublicSetterCount = typeof(FullName).GetProperties().Count(x => x.GetSetMethod() != null);
            propertiesWithPublicSetterCount.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow("First", "Last")]
        [DataRow("First", "Last")]
        public void CreateFullNameShouldSucceed(string firstNameValue, string lastNameValue)
        {
            // Act
            var firstName = new FirstName(firstNameValue);
            var lastName = new LastName(lastNameValue);
            var result = new FullName(firstName, lastName);

            // Assert
            result.FirstName.Should().Be(firstName);
            result.LastName.Should().Be(lastName);
        }

        [DataTestMethod]
        [DataRow("First", "Last")]
        public void ChangeFirstNameAndLastNameShouldSucceed(string firstNameValue, string lastNameValue)
        {
            // Act
            var firstName = new FirstName(firstNameValue);
            var lastName = new LastName(lastNameValue);
            var result = new FullName(firstName, lastName);

            var updatedFirstNameValue = $"{firstNameValue}Updated";
            var updatedLastNameValue = $"{lastNameValue}Updated";

            // Act
            result.ChangeFirstName(updatedFirstNameValue);
            result.ChangeLastName(updatedLastNameValue);

            // Assert
            result.FirstName.Should().NotBe(firstName);
            result.LastName.Should().NotBe(lastName);
        }
    }
}
