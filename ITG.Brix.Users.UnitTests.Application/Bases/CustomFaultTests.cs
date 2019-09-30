using FluentAssertions;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Application.Bases
{
    [TestClass]
    public class CustomFaultTests
    {
        [TestMethod]
        public void ShouldHaveCustomErrorType()
        {
            // Arrange
            var fault = new CustomFault();

            // Act
            var type = fault.Type;

            // Assert
            type.Should().Be(ErrorType.CustomError);
        }
    }
}
