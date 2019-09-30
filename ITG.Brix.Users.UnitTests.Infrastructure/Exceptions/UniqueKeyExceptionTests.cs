using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Exceptions
{
    [TestClass]
    public class UniqueKeyExceptionTests
    {
        [TestMethod]
        public void ShouldHavePredefinedMessage()
        {
            // Arrange
            var expectedMessage = "UniqueKey";

            // Act
            var exception = new UniqueKeyException();

            // Assert
            exception.Message.Should().Be(expectedMessage);
        }
    }
}
