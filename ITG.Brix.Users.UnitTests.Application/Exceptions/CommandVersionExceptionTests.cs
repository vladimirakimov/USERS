using FluentAssertions;
using ITG.Brix.Users.Application.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Application.Exceptions
{
    [TestClass]
    public class CommandVersionExceptionTests
    {
        [TestMethod]
        public void ShouldHavePredefinedMessage()
        {
            // Arrange
            var expectedMessage = "CommandVersion";

            // Act
            var exception = new CommandVersionException();

            // Assert
            exception.Message.Should().Be(expectedMessage);
        }
    }
}
