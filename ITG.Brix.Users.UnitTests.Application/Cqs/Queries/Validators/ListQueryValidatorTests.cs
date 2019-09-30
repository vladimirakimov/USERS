using FluentAssertions;
using ITG.Brix.Users.Application.Constants;
using ITG.Brix.Users.Application.Cqs.Queries;
using ITG.Brix.Users.Application.Cqs.Queries.Validators;
using ITG.Brix.Users.Application.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Queries.Validators
{
    [TestClass]
    public class ListQueryValidatorTests
    {
        private ListQueryValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new ListQueryValidator();
        }

        [TestMethod]
        public void ShouldContainNoErrors()
        {
            // Arrange
            string filter = null;
            string top = null;
            string skip = null;
            var query = new ListQuery(filter, top, skip);

            // Act
            var validationResult = _validator.Validate(query);
            var exists = validationResult.Errors.Count > 0;

            // Assert
            exists.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow(null)]
        public void ShouldContainNoTopErrorWhenTopIsNotSet(string top)
        {
            // Arrange
            string filter = null;
            string skip = null;

            var query = new ListQuery(filter, top, skip);

            // Act
            var validationResult = _validator.Validate(query);
            var exists = validationResult.Errors.Any(a => a.PropertyName.Equals("$top"));

            // Assert
            exists.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("0")]
        [DataRow("-1")]
        [DataRow("99999999999999999999999")]
        public void ShouldHaveTopRangeCustomFailure(string top)
        {
            // Arrange
            string filter = null;
            string skip = null;

            var query = new ListQuery(filter, top, skip);

            // Act
            var validationResult = _validator.Validate(query);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("$top") && a.ErrorMessage.Contains(string.Format(CustomFailures.TopRange, Consts.CqsValidation.TopMaxValue)));

            // Assert
            exists.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("some invalid value - not a sequence of digits")]
        [DataRow("null")]
        [DataRow("")]
        [DataRow("  ")]
        public void ShouldHaveTopInvalidCustomFailureWhenTopInvalid(string top)
        {
            // Arrange
            string filter = null;
            string skip = null;

            var query = new ListQuery(filter, top, skip);

            // Act
            var validationResult = _validator.Validate(query);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("$top") && a.ErrorMessage.Contains(CustomFailures.TopInvalid));

            // Assert
            exists.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow(null)]
        public void ShouldContainNoSkipErrorWhenSkipIsNotSet(string skip)
        {
            // Arrange
            string filter = null;
            string top = null;

            var query = new ListQuery(filter, top, skip);

            // Act
            var validationResult = _validator.Validate(query);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("$skip"));

            // Assert
            exists.Should().BeFalse();
        }

        [DataTestMethod]
        [DataRow("-1")]
        [DataRow("99999999999999999999999")]
        public void ShouldHaveSkipRangeCustomFailure(string skip)
        {
            // Arrange
            string filter = null;
            string top = null;

            var query = new ListQuery(filter, top, skip);

            // Act
            var validationResult = _validator.Validate(query);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("$skip") && a.ErrorMessage.Contains(string.Format(CustomFailures.SkipRange, Consts.CqsValidation.SkipMaxValue)));

            // Assert
            exists.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("some invalid value - not a sequence of digits")]
        [DataRow("null")]
        [DataRow("")]
        [DataRow("  ")]
        public void ShouldHaveSkipInvalidCustomFailureWhenSkipInvalid(string skip)
        {
            // Arrange
            string filter = null;
            string top = null;

            var query = new ListQuery(filter, top, skip);

            // Act
            var validationResult = _validator.Validate(query);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("$skip") && a.ErrorMessage.Contains(CustomFailures.SkipInvalid));

            // Assert
            exists.Should().BeTrue();
        }
    }
}
