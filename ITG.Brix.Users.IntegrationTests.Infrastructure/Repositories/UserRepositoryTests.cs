using FluentAssertions;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps;
using ITG.Brix.Users.Infrastructure.Configurations.Impl;
using ITG.Brix.Users.Infrastructure.Exceptions;
using ITG.Brix.Users.Infrastructure.Repositories;
using ITG.Brix.Users.IntegrationTests.Infrastructure.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ITG.Brix.Users.IntegrationTests.Infrastructure.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        private IUserRepository _repository;

        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
#if DEBUG
            ClassMapsRegistrator.RegisterMaps();
#endif
        }

        [TestInitialize]
        public void TestInitialize()
        {
#if DEBUG
            RepositoryTestsHelper.Init();
            _repository = new UserRepository(new PersistenceContext(new PersistenceConfiguration(RepositoryTestsHelper.ConnectionString)));
#endif
        }

        [TestMethod]
        public async Task CreateShouldSucceed()
        {
#if DEBUG
            // Arrange
            var id = Guid.NewGuid();
            var login = new Login("login");
            var password = "password";
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);
            var user = new User(id, login, password, fullName);

            // Act
            await _repository.Create(user);

            // Assert
            var data = RepositoryHelper.GetUsers();
            data.Should().HaveCount(1);
            var result = data.First();
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Login.Should().Be(login);
            result.Password.Should().Be(password);
            result.FullName.FirstName.Should().Be(firstName);
            result.FullName.LastName.Should().Be(lastName);
#endif
        }

        [TestMethod]
        public async Task CreateWithAlreadyExistingLoginShouldFail()
        {
#if DEBUG
            // Arrange
            var id = Guid.NewGuid();
            var login = new Login("notUniqueLogin");
            var password = "password";
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);
            var user = new User(id, login, password, fullName);

            await _repository.Create(user);

            // Act
            Action act = () => { _repository.Create(user).GetAwaiter().GetResult(); };

            // Assert
            act.Should().Throw<UniqueKeyException>();
#endif
        }

        [TestMethod]
        public async Task UpdateShouldSucceed()
        {
#if DEBUG
            // Arrange
            var id = Guid.NewGuid();
            var loginValue = "login";
            var password = "password";
            var passwordNew = "passwordNew";
            var firstNameValue = "FirstName";
            var lastNameValue = "LastName";
            RepositoryHelper.CreateUser(id, loginValue, password, firstNameValue, lastNameValue);

            var login = new Login(loginValue);
            var firstName = new FirstName(firstNameValue);
            var lastName = new LastName(lastNameValue);
            var fullName = new FullName(firstName, lastName);
            var user = new User(id, login, password, fullName);

            user.ChangePassword(passwordNew);

            // Act
            await _repository.Update(user);

            // Assert
            var data = RepositoryHelper.GetUsers();
            data.Should().HaveCount(1);
            var result = data.First();
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.Login.Should().Be(login);
            result.Password.Should().Be(passwordNew);
            result.FullName.FirstName.Should().Be(firstName);
            result.FullName.LastName.Should().Be(lastName);
#endif
        }

        [TestMethod]
        public void UpdateWithAlreadyExistingLoginShouldFail()
        {
#if DEBUG
            // Arrange
            var id = Guid.NewGuid();
            var loginValue = "loginOne";
            var password = "password";
            var firstName = "FirstName";
            var lastName = "LastName";
            RepositoryHelper.CreateUser(id, loginValue, password, firstName, lastName);

            var otherId = Guid.NewGuid();
            var otherLoginValue = "loginTwo";
            var otherPassword = "password";
            var otherFirstName = "FirstName";
            var otherLastName = "LastName";
            RepositoryHelper.CreateUser(otherId, otherLoginValue, otherPassword, otherFirstName, otherLastName);

            var data = RepositoryHelper.GetUsers();

            var other = data.Where(x => x.Login.Value == "loginTwo").First();
            var loginOne = new Login("loginOne");
            other.ChangeLogin(loginOne);

            // Act
            Action act = () => { _repository.Update(other).GetAwaiter().GetResult(); };

            // Assert
            act.Should().Throw<UniqueKeyException>();
#endif
        }

        [TestMethod]
        public async Task DeleteShouldSucceed()
        {
#if DEBUG
            // Arrange
            var id = Guid.NewGuid();
            var login = "login";
            var password = "password";
            var firstName = "FirstName";
            var lastName = "LastName";
            RepositoryHelper.CreateUser(id, login, password, firstName, lastName);

            // Act
            await _repository.Delete(id, 0);

            // Assert
            var data = RepositoryHelper.GetUsers();
            data.Should().HaveCount(0);
#endif
        }

    }
}
