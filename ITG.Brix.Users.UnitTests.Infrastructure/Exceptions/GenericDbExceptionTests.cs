using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Exceptions
{
    [TestClass]
    public class GenericDbExceptionTests
    {
        [TestMethod]
        public void ShouldHavePredefinedMessage()
        {
            // Arrange
            var expectedMessage = "Generic";
            string parameter;
            var argumentException = new ArgumentException(nameof(parameter));

            // Act
            var exception = new GenericDbException(argumentException);

            // Assert
            exception.Message.Should().Be(expectedMessage);
        }
    }
}
