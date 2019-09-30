using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Providers;
using ITG.Brix.Users.Infrastructure.Providers.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Providers
{
    [TestClass]
    public class JsonProviderTests
    {
        [TestMethod]
        public void GenerateShouldBeInRange()
        {
            // Arrange
            IJsonProvider jsonProvider = new JsonProvider();
            var json = @"{
                                    ""firstName"" : ""TheFirstName"",
                                    ""lastName"" : ""TheLastName""
                                 }";

            // Act
            var result = jsonProvider.ToDictionary(json);

            // Assert
            result.Should().NotBeNull();
            result.Keys.Count.Should().Be(2);
            result["firstName"].Should().Be("TheFirstName");
            result["lastName"].Should().Be("TheLastName");
        }
    }
}
