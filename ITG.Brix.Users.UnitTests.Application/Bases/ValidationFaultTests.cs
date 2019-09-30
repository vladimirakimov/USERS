using FluentAssertions;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Application.Bases
{
    [TestClass]
    public class ValidationFaultTests
    {
        [TestMethod]
        public void ShouldHaveValidationErrorType()
        {
            // Arrange
            var fault = new ValidationFault();

            // Act
            var type = fault.Type;

            // Assert
            type.Should().Be(ErrorType.ValidationError);
        }
    }
}
