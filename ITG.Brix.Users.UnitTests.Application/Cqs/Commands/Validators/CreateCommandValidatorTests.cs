using FluentAssertions;
using ITG.Brix.Users.Application.Constants;
using ITG.Brix.Users.Application.Cqs.Commands;
using ITG.Brix.Users.Application.Cqs.Commands.Validators;
using ITG.Brix.Users.Application.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Commands.Validators
{
    [TestClass]
    public class CreateCommandValidatorTests
    {
        private CreateCommandValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new CreateCommandValidator();
        }

        [TestMethod]
        public void ShouldContainNoErrors()
        {
            // Arrange
            var command = new CreateCommand(login: "Login", password: "Password$my", firstName: "First Name", lastName: "Last Name");

            // Act
            var validationResult = _validator.Validate(command);
            var exists = validationResult.Errors.Count > 0;

            // Assert
            exists.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldHaveLoginMandatoryValidationFailureWhenLoginIsNull()
        {
            // Arrange
            string login = null;

            var command = new CreateCommand(login: login, password: "Password$my", firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginMandatory));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLoginMandatoryValidationFailureWhenLoginIsEmpty()
        {
            // Arrange
            var login = string.Empty;

            var command = new CreateCommand(login: login, password: "Password$my", firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginMandatory));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLoginMandatoryValidationFailureWhenLoginIsWhitespace()
        {
            // Arrange
            var login = "  ";

            var command = new CreateCommand(login: login, password: "Password", firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginMandatory));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLoginLegthValidationFailureWhenLengthIsLessThanMinimumAllowed()
        {
            // Arrange
            var login = new string('c', Consts.CqsValidation.LoginLengthMin - 1);
            var command = new CreateCommand(login: login, password: "Password", firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLoginLegthValidationFailureWhenLengthIsGreaterThanMaximumAllowed()
        {
            // Arrange
            var login = new string('c', Consts.CqsValidation.LoginLengthMax + 1);
            var command = new CreateCommand(login: login, password: "Password", firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginLength));

            // Assert
            exists.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow(" ")]
        public void ShouldHaveLoginFirstLetterValidationFailureWhenFirstCharacterIsNotLetter(string symbol)
        {
            // Arrange
            var login = symbol + new string('c', Consts.CqsValidation.LoginLengthMin + 1);
            var command = new CreateCommand(login: login, password: "Password", firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginFirstLetter));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordMandatoryValidationFailureWhenPasswordIsNull()
        {
            // Arrange
            string password = null;

            var command = new CreateCommand(login: "Login", password: password, firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordMandatory));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordMandatoryValidationFailureWhenPasswordIsEmpty()
        {
            // Arrange
            var password = string.Empty;

            var command = new CreateCommand(login: "Login", password: password, firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordMandatory));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordLegthValidationFailureWhenLengthIsLessThanMinimumAllowed()
        {
            // Arrange
            var password = new string('p', Consts.CqsValidation.PasswordLengthMin - 1);

            var command = new CreateCommand(login: "Login", password: password, firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordLegthValidationFailureWhenLengthIsGreaterThanMaximumAllowed()
        {
            // Arrange
            var password = new string('p', Consts.CqsValidation.PasswordLengthMax + 1);

            var command = new CreateCommand(login: "Login", password: password, firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordSpecialValidationFailureWhenNoSpecialCharacterPresent()
        {
            // Arrange
            var password = new string('p', Consts.CqsValidation.PasswordLengthMin + 1);

            var command = new CreateCommand(login: "Login", password: password, firstName: "FirstName", lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordSpecial));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameLengthValidationFailureWhenFirstNameIsNull()
        {
            // Arrange
            string firstName = null;

            var command = new CreateCommand(login: "Login", password: "Password$my", firstName: firstName, lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameValidationFailureWhenFirstNameIsEmpty()
        {
            // Arrange
            var firstName = string.Empty;

            var command = new CreateCommand(login: "Login", password: "Password$my", firstName: firstName, lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameValidationFailureWhenFirstNameIsWhiteSpace()
        {
            // Arrange
            var firstName = "   ";

            var command = new CreateCommand(login: "Login", password: "Password$my", firstName: firstName, lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameLegthValidationFailureWhenLengthIsLessThanMinimumAllowed()
        {
            // Arrange
            var firstName = new string('f', Consts.CqsValidation.FirstNameLengthMin - 1);

            var command = new CreateCommand(login: "Login", password: "Password$my", firstName: firstName, lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName") && a.ErrorMessage.Contains(ValidationFailures.FirstNameLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameLegthValidationFailureWhenLengthIsGreaterThanMaximumAllowed()
        {
            // Arrange
            var firstName = new string('f', Consts.CqsValidation.FirstNameLengthMax + 1);

            var command = new CreateCommand(login: "Login", password: "Password$my", firstName: firstName, lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName") && a.ErrorMessage.Contains(ValidationFailures.FirstNameLength));

            // Assert
            exists.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow(" ")]
        public void ShouldHaveFirstNameOnlyLettersAllowedValidationFailureWhenContainsSymbolsOtherThanLetters(string symbol)
        {
            // Arrange
            var firstName = new string('f', Consts.CqsValidation.FirstNameLengthMin) + symbol;

            var command = new CreateCommand(login: "Login", password: "Password$my", firstName: firstName, lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName") && a.ErrorMessage.Contains(ValidationFailures.FirstNameOnlyLettersAndSingleSpacesAllowed));

            // Assert
            exists.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow(" ")]
        public void ShouldHaveFirstNameOnlyLettersAndSingleSpacesAllowedValidationFailureWhenContainsSymbolsOtherThanLettersAndSingleSpaces(string symbol)
        {
            // Arrange
            var firstName = "f ff" + symbol;

            var command = new CreateCommand(login: "Login", password: "Password$my", firstName: firstName, lastName: "LastName");

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName") && a.ErrorMessage.Contains(ValidationFailures.FirstNameOnlyLettersAndSingleSpacesAllowed));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameValidationFailureWhenValueIsNull()
        {
            // Arrange
            string lastName = null;

            var command = new CreateCommand(login: "Login", password: "Password$Secret", firstName: "FirstName", lastName: lastName);

            // Act
            var validationResult = _validator.Validate(command);
            var exists = validationResult.Errors.Any(a => a.PropertyName.Equals("LastName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameValidationFailureWhenValueIsEmpty()
        {
            // Arrange
            var lastName = string.Empty;

            var command = new CreateCommand(login: "Login", password: "Password$Secret", firstName: "FirstName", lastName: lastName);

            // Act
            var validationResult = _validator.Validate(command);
            var exists = validationResult.Errors.Any(a => a.PropertyName.Equals("LastName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameValidationFailureWhenValueIsWhiteSpace()
        {
            // Arrange
            var lastName = "   ";

            var command = new CreateCommand(login: "Login", password: "Password$Secret", firstName: "FirstName", lastName: lastName);

            // Act
            var validationResult = _validator.Validate(command);
            var exists = validationResult.Errors.Any(a => a.PropertyName.Equals("LastName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameLegthValidationFailureWhenLengthIsLessThanMinimumAllowed()
        {
            // Arrange
            var lastName = new string('l', Consts.CqsValidation.LastNameLengthMin - 1);
            var command = new CreateCommand(login: "Login", password: "Password$Secret", firstName: "FirstName", lastName: lastName);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("LastName") && a.ErrorMessage.Contains(ValidationFailures.LastNameLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameLegthValidationFailureWhenLengthIsGreaterThanMaximumAllowed()
        {
            // Arrange
            var lastName = new string('l', Consts.CqsValidation.FirstNameLengthMax + 1);
            var command = new CreateCommand(login: "Login", password: "Password$Secret", firstName: "FirstName", lastName: lastName);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("LastName") && a.ErrorMessage.Contains(ValidationFailures.LastNameLength));

            // Assert
            exists.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow(" ")]
        public void ShouldHaveLastNameOnlyLettersAllowedValidationFailureWhenContainsSymbolsOtherThanLetters(string symbol)
        {
            // Arrange
            var lastName = new string('l', Consts.CqsValidation.LastNameLengthMin + 1) + symbol;
            var command = new CreateCommand(login: "Login", password: "Password$Secret", firstName: "FirstName", lastName: lastName);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("LastName") && a.ErrorMessage.Contains(ValidationFailures.LastNameOnlyLettersAndSingleSpacesAllowed));

            // Assert
            exists.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow(" ")]
        public void ShouldHaveLastNameOnlyLettersAndSingleSpacesAllowedValidationFailureWhenContainsSymbolsOtherThanLettersAndSingleSpaces(string symbol)
        {
            // Arrange
            var lastName = "l ll" + symbol;
            var command = new CreateCommand(login: "Login", password: "Password$Secret", firstName: "FirstName", lastName: lastName);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("LastName") && a.ErrorMessage.Contains(ValidationFailures.LastNameOnlyLettersAndSingleSpacesAllowed));

            // Assert
            exists.Should().BeTrue();
        }
    }
}
