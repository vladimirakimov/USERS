using FluentAssertions;
using ITG.Brix.Users.Application.Cqs.Queries;
using ITG.Brix.Users.Application.Cqs.Queries.Validators;
using ITG.Brix.Users.Application.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Queries.Validators
{
    [TestClass]
    public class GetQueryValidatorTests
    {
        private GetQueryValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new GetQueryValidator();
        }

        [TestMethod]
        public void ShouldContainNoErrors()
        {
            // Arrange
            var query = new GetQuery(id: Guid.NewGuid());

            // Act
            var validationResult = _validator.Validate(query);
            var exists = validationResult.Errors.Count > 0;

            // Assert
            exists.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldHaveUserNotFoundCustomFailureWhenIdIsGuidEmpty()
        {
            // Arrange
            var query = new GetQuery(id: Guid.Empty);

            // Act
            var validationResult = _validator.Validate(query);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Id") && a.ErrorMessage.Contains(CustomFailures.UserNotFound));

            // Assert
            exists.Should().BeTrue();
        }
    }
}
