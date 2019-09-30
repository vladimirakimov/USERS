using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Exceptions
{
    [TestClass]
    public class EntityNotFoundDbExceptionTests
    {
        [TestMethod]
        public void ShouldHavePredefinedMessage()
        {
            // Arrange
            var expectedMessage = "EntityNotFound";

            // Act
            var exception = new EntityNotFoundDbException();

            // Assert
            exception.Message.Should().Be(expectedMessage);
        }
    }
}
