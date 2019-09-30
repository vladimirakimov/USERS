using FluentAssertions;
using ITG.Brix.Users.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.Domain
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void CreateUserShouldSucceed()
        {
            // Arrange
            var id = Guid.NewGuid();
            var login = new Login("login");
            var password = "password";
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);

            // Act
            var result = new User(id, login, password, fullName);

            // Assert
            result.Id.Should().Be(id);
            result.Login.Should().Be(login);
            result.Password.Should().Be(password);
        }

        [TestMethod]
        public void CreateUserShouldFailWhenIdIsDefaultGuid()
        {
            // Arrange
            var id = default(Guid);
            var login = new Login("login");
            var password = "password";
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);

            // Act
            Action ctor = () => { new User(id, login, password, fullName); };

            // Assert
            ctor.Should().Throw<ArgumentException>().WithMessage($"*{nameof(id)}*");
        }

        [TestMethod]
        public void CreateUserShouldFailWhenLoginIsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            Login login = null;
            var password = "password";
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);

            // Act
            Action ctor = () => { new User(id, login, password, fullName); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>().WithMessage($"*{nameof(login)}*");
        }

        [TestMethod]
        public void CreateUserShouldFailWhenPasswordIsStringEmpty()
        {
            // Arrange
            var id = Guid.NewGuid();
            var login = new Login("login");
            var password = string.Empty;
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);

            // Act
            Action ctor = () => { new User(id, login, password, fullName); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>().WithMessage($"*{nameof(password)}*");
        }

        [TestMethod]
        public void CreateUserShouldFailWhenPasswordIsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var login = new Login("login");
            string password = null;
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);

            // Act
            Action ctor = () => { new User(id, login, password, fullName); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>().WithMessage($"*{nameof(password)}*");
        }

        [TestMethod]
        public void UsersAreEqualWhenTheyHaveSameLoginValues()
        {
            var login = new Login("login");
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);
            var user1 = new User(Guid.NewGuid(), login, "password-1", fullName);
            var user2 = new User(Guid.NewGuid(), login, "password-2", fullName);
            user1.Should().Be(user2);
        }

        [TestMethod]
        public void UsersAreDifferentWhenTheyHaveDifferentLogin()
        {
            var id = Guid.NewGuid();
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);
            var login1 = new Login("login 1");
            var login2 = new Login("login 2");
            var user1 = new User(id, login1, "password", fullName);
            var user2 = new User(id, login2, "password", fullName);

            user1.Should().NotBe(user2);
        }
    }
}

