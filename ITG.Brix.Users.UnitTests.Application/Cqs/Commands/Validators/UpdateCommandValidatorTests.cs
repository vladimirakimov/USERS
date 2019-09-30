using FluentAssertions;
using ITG.Brix.Users.Application.Constants;
using ITG.Brix.Users.Application.Cqs.Commands;
using ITG.Brix.Users.Application.Cqs.Commands.Validators;
using ITG.Brix.Users.Application.DataTypes;
using ITG.Brix.Users.Application.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Commands.Validators
{
    [TestClass]
    public class UpdateCommandValidatorTests
    {
        private UpdateCommandValidator _validator;

        [TestInitialize]
        public void TestInitialize()
        {
            _validator = new UpdateCommandValidator();
        }

        [TestMethod]
        public void ShouldContainNoErrors()
        {
            // Arrange
            Optional<string> login = new Optional<string>("Login");
            Optional<string> password = new Optional<string>("Password$my");
            Optional<string> firstName = new Optional<string>("FirstName");
            Optional<string> lastName = new Optional<string>("LastName");

            var command = new UpdateCommand(id: Guid.NewGuid(),
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

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
            Optional<string> login = new Optional<string>("Login");
            Optional<string> password = new Optional<string>("Password");
            Optional<string> firstName = new Optional<string>("FirstName");
            Optional<string> lastName = new Optional<string>("LastName");

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Id") && a.ErrorMessage.Contains(CustomFailures.UserNotFound));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLoginCannotBeEmptyValidationErrorWhenLoginIsNull()
        {
            // Arrange
            string loginValue = null;

            Optional<string> login = new Optional<string>(loginValue);
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginCannotBeEmpty));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLoginCannotBeEmptyValidationErrorWhenLoginIsEmpty()
        {
            // Arrange
            var loginValue = string.Empty;

            Optional<string> login = new Optional<string>(loginValue);
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginCannotBeEmpty));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLoginCannotBeEmptyValidationErrorWhenLoginIsWhiteSpace()
        {
            // Arrange
            var loginValue = "   ";

            Optional<string> login = new Optional<string>(loginValue);
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginCannotBeEmpty));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLoginLegthValidationErrorWhenLengthIsLessThanMinimumAllowed()
        {
            // Arrange
            var loginValue = new string('c', Consts.CqsValidation.LoginLengthMin - 1);

            Optional<string> login = new Optional<string>(loginValue);
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLoginLegthValidationErrorWhenLengthIsGreaterThanMaximumAllowed()
        {
            // Arrange
            var loginValue = new string('c', Consts.CqsValidation.LoginLengthMax + 1);

            Optional<string> login = new Optional<string>(loginValue);
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

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
        public void ShouldHaveLoginFirstLetterValidationErrorWhenFirstCharacterIsNotLetter(string symbol)
        {
            // Arrange
            var loginValue = symbol + new string('#', Consts.CqsValidation.LoginLengthMin - 1);

            Optional<string> login = new Optional<string>(loginValue);
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Login") && a.ErrorMessage.Contains(ValidationFailures.LoginFirstLetter));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordCannotBeEmptyValidationErrorWhenPasswordHasValueNull()
        {
            // Arrange
            string passwordValue = null;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>(passwordValue);
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordCannotBeEmpty));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordCannotBeEmptyValidationErrorWhenPasswordHasValueEmpty()
        {
            // Arrange
            var passwordValue = string.Empty;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>(passwordValue);
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordCannotBeEmpty));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordCannotBeEmptyValidationErrorWhenPasswordHasValueWhitespace()
        {
            // Arrange
            var passwordValue = "   ";

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>(passwordValue);
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordCannotBeEmpty));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordLegthValidationErrorWhenLengthIsLessThanMinimumAllowed()
        {
            // Arrange
            var passwordValue = new string('p', Consts.CqsValidation.PasswordLengthMin - 1);

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>(passwordValue);
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordLegthValidationErrorWhenLengthIsGreaterThanMaximumAllowed()
        {
            // Arrange
            var passwordValue = new string('p', Consts.CqsValidation.PasswordLengthMax + 1);

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>(passwordValue);
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHavePasswordSpecialValidationErrorWhenNoSpecialCharacterPresent()
        {
            // Arrange
            var passwordValue = new string('p', Consts.CqsValidation.PasswordLengthMin);

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>(passwordValue);
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("Password") && a.ErrorMessage.Contains(ValidationFailures.PasswordSpecial));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameValidationErrorWhenFirstNameIsNull()
        {
            // Arrange
            string firstNameValue = null;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>(firstNameValue);
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameValidationErrorWhenFirstNameIsEmpty()
        {
            // Arrange
            var firstNameValue = string.Empty;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>(firstNameValue);
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameValidationErrorWhenFirstNameIsWhitespace()
        {
            // Arrange
            var firstNameValue = "   ";

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>(firstNameValue);
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameLegthValidationErrorWhenLengthIsLessThanMinimumAllowed()
        {
            // Arrange
            var firstNameValue = new string('f', Consts.CqsValidation.FirstNameLengthMin - 1);

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>(firstNameValue);
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName") && a.ErrorMessage.Contains(ValidationFailures.FirstNameLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveFirstNameLegthValidationErrorWhenLengthIsGreaterThanMaximumAllowed()
        {
            // Arrange
            var firstNameValue = new string('f', Consts.CqsValidation.FirstNameLengthMax + 1);

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>(firstNameValue);
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

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
        public void ShouldHaveFirstNameOnlyLettersAllowedValidationErrorWhenContainsSymbolsOtherThanLetters(string symbol)
        {
            var firstNameValue = new string('f', Consts.CqsValidation.FirstNameLengthMin) + symbol;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>(firstNameValue);
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);
            var validationResult = _validator.Validate(command);

            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName") && a.ErrorMessage.Contains(ValidationFailures.FirstNameOnlyLettersAndSingleSpacesAllowed));

            exists.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow("1")]
        [DataRow("#")]
        [DataRow(" ")]
        public void ShouldHaveFirstNameOnlyLettersAndSingleSpacesAllowedValidationErrorWhenContainsSymbolsOtherThanLettersAndSingleSpaces(string symbol)
        {
            var firstNameValue = "f ff" + symbol;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>(firstNameValue);
            Optional<string> lastName = new Optional<string>();

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);
            var validationResult = _validator.Validate(command);

            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("FirstName") && a.ErrorMessage.Contains(ValidationFailures.FirstNameOnlyLettersAndSingleSpacesAllowed));

            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameValidationErrorWhenValueIsNull()
        {
            // Arrange
            string lastNameValue = null;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>(lastNameValue);

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("LastName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameValidationErrorWhenValueIsEmpty()
        {
            // Arrange
            var lastNameValue = string.Empty;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>(lastNameValue);

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("LastName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameValidationErrorWhenFirstNameIsWhitespace()
        {
            // Arrange
            var lastNameValue = "   ";

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>(lastNameValue);

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("LastName"));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameLegthValidationErrorWhenLengthIsLessThanMinimumAllowed()
        {
            // Arrange
            var lastNameValue = new string('l', Consts.CqsValidation.LastNameLengthMin - 1);

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>(lastNameValue);

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

            // Act
            var validationResult = _validator.Validate(command);
            var exists =
                validationResult.Errors.Any(
                    a => a.PropertyName.Equals("LastName") && a.ErrorMessage.Contains(ValidationFailures.LastNameLength));

            // Assert
            exists.Should().BeTrue();
        }

        [TestMethod]
        public void ShouldHaveLastNameLegthValidationErrorWhenLengthIsGreaterThanMaximumAllowed()
        {
            // Arrange
            var lastNameValue = new string('l', Consts.CqsValidation.LastNameLengthMax + 1);

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>(lastNameValue);

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

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
        public void ShouldHaveLastNameOnlyLettersAllowedValidationErrorWhenContainsSymbolsOtherThanLetters(string symbol)
        {
            // Arrange
            var lastNameValue = new string('l', Consts.CqsValidation.LastNameLengthMin) + symbol;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>(lastNameValue);

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

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
        public void ShouldHaveLastNameOnlyLettersAndSingleSpacesAllowedValidationErrorWhenContainsSymbolsOtherThanLettersAndSingleSpaces(string symbol)
        {
            // Arrange
            var lastNameValue = "l ll" + symbol;

            Optional<string> login = new Optional<string>();
            Optional<string> password = new Optional<string>();
            Optional<string> firstName = new Optional<string>();
            Optional<string> lastName = new Optional<string>(lastNameValue);

            var command = new UpdateCommand(id: Guid.Empty,
                                            login: login,
                                            password: password,
                                            firstName: firstName,
                                            lastName: lastName,
                                            version: 0);

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
