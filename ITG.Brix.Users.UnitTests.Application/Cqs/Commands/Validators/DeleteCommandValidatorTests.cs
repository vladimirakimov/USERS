using FluentAssertions;
using ITG.Brix.Users.Application.Cqs.Commands;
using ITG.Brix.Users.Application.Cqs.Commands.Validators;
using ITG.Brix.Users.Application.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Commands.Validators
{
    [TestClass]
    public class DeleteCommandValidatorTests
    {
        private DeleteCommandValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new DeleteCommandValidator();
        }

        [TestMethod]
        public void ShouldContainNoErrors()
        {
            // Arrange
            var command = new DeleteCommand(id: Guid.NewGuid(), version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists = validationResult.Errors.Count > 0;

            // Assert
            exists.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldHaveUserNotFoundCustomFailureWhenIdIsGuidEmpty()
        {
            // Arrange
            var command = new DeleteCommand(id: Guid.Empty, version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Id") && a.ErrorMessage.Contains(CustomFailures.UserNotFound));

            // Assert
            exists.Should().BeTrue();
        }
    }
}
