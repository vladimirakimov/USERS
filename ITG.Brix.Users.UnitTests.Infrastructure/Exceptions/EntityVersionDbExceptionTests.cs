using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Exceptions
{
    [TestClass]
    public class EntityVersionDbExceptionTests
    {
        [TestMethod]
        public void ShouldHavePredefinedMessage()
        {
            // Arrange
            var expectedMessage = "EntityVersion";

            // Act
            var exception = new EntityVersionDbException();

            // Assert
            exception.Message.Should().Be(expectedMessage);
        }
    }
}
