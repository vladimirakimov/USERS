using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Exceptions
{
    [TestClass]
    public class FilterODataExceptionTests
    {
        [TestMethod]
        public void ShouldHavePredefinedMessage()
        {
            // Arrange
            var expectedMessage = "FilterOData";

            // Act
            var exception = new FilterODataException();

            // Assert
            exception.Message.Should().Be(expectedMessage);
        }
    }
}
