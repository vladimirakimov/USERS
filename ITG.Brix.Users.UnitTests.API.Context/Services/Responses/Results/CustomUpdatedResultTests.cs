using FluentAssertions;
using ITG.Brix.Users.API.Context.Services.Responses.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Responses.Results
{
    [TestClass]
    public class CustomUpdatedResultTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var eTagValue = "234234324325";

            // Act
            var obj = new CustomUpdatedResult(eTagValue);

            // Assert
            obj.Should().NotBeNull();
        }

        [TestMethod]
        public void ConstructorShouldSetETagValue()
        {
            // Arrange
            var eTagValue = "234234324325";

            // Act
            var obj = new CustomUpdatedResult(eTagValue);

            // Assert
            obj.ETagValue.Should().Be(eTagValue);
        }
    }
}
