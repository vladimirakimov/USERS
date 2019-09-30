using FluentAssertions;
using ITG.Brix.Users.API.Context.Services.Responses.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Responses.Results
{
    [TestClass]
    public class CustomCreatedResultTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var location = string.Format("/users/{0}", Guid.NewGuid());
            var eTagValue = "234234324325";

            // Act
            var obj = new CustomCreatedResult(location, eTagValue);

            // Assert
            obj.Should().NotBeNull();
        }

        [TestMethod]
        public void ConstructorShouldSetETagValue()
        {
            // Arrange
            var location = string.Format("/users/{0}", Guid.NewGuid());
            var eTagValue = "234234324325";

            // Act
            var obj = new CustomCreatedResult(location, eTagValue);

            // Assert
            obj.ETagValue.Should().Be(eTagValue);
        }
    }
}
