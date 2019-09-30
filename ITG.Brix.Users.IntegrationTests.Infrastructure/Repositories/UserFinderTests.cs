using FluentAssertions;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps;
using ITG.Brix.Users.Infrastructure.Configurations.Impl;
using ITG.Brix.Users.Infrastructure.Exceptions;
using ITG.Brix.Users.Infrastructure.Repositories;
using ITG.Brix.Users.IntegrationTests.Infrastructure.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ITG.Brix.Users.IntegrationTests.Infrastructure.Repositories
{
    [TestClass]
    public class UserFinderTests
    {
        private IUserFinder _finder;

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
            _finder = new UserFinder(new PersistenceContext(new PersistenceConfiguration(RepositoryTestsHelper.ConnectionString)));
#endif
        }

        [TestMethod]
        public async Task GetByIdShouldSucceed()
        {
#if DEBUG
            // Arrange
            var id = Guid.NewGuid();
            RepositoryHelper.CreateUser(id, "login", "password", "FirstName", "LastName");

            // Act
            var result = await _finder.Get(id);

            // Assert
            result.Should().NotBeNull();
#endif
        }

        [TestMethod]
        public void GetByNonExistentIdShouldThrowEntityNotFoundDbException()
        {
#if DEBUG
            // Arrange
            var nonExistentUserId = Guid.NewGuid();

            // Act
            Func<Task> call = async () => { await _finder.Get(nonExistentUserId); };

            // Assert
            call.Should().Throw<EntityNotFoundDbException>();
#endif
        }

        [TestMethod]
        public async Task GetByLoginShouldSucceed()
        {
#if DEBUG
            // Arrange
            var login = "loginu";
            RepositoryHelper.CreateUser(Guid.NewGuid(), login, "password", "FirstName", "LastName");

            // Act
            var result = await _finder.Get(login);

            // Assert
            result.Should().NotBeNull();
#endif
        }

        [TestMethod]
        public void GetByNonExistentLoginShouldThrowEntityNotFoundDbException()
        {
#if DEBUG
            // Arrange
            var nonExistentUserLogin = "nonExistentUserLogin";

            // Act
            Func<Task> call = async () => { await _finder.Get(nonExistentUserLogin); };

            // Assert
            call.Should().Throw<EntityNotFoundDbException>();
#endif
        }

        [TestMethod]
        public async Task ListShouldReturnAllRecords()
        {
#if DEBUG
            // Arrange
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-1", "password", "FirstName", "LastName");
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-2", "password", "FirstName", "LastName");
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-3", "password", "FirstName", "LastName");

            // Act
            var result = await _finder.List(null, null, null);

            // Assert
            result.Should().HaveCount(3);
#endif
        }

        [TestMethod]
        public async Task ListShouldReturnFilteredResult()
        {
#if DEBUG
            // Arrange
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-1", "password", "FirstName", "LastName");
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-2", "password", "FirstName", "LastName");
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-3", "password", "FirstName", "LastName");

            Expression<Func<User, bool>> filter = x => x.Login.Value == "login-1";

            // Act
            var result = await _finder.List(filter, null, null);

            // Assert
            result.Should().HaveCount(1);
#endif
        }

        [TestMethod]
        public async Task ListShouldReturnCorrectResultWithSkipOnly()
        {
#if DEBUG
            // Arrange
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-1", "password", "FirstName", "LastName");
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-2", "password", "FirstName", "LastName");
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-3", "password", "FirstName", "LastName");

            int? skip = 2;

            // Act
            var result = await _finder.List(null, skip, null);

            // Assert
            result.Should().HaveCount(1);
#endif
        }

        [TestMethod]
        public async Task ListShouldReturnCorrectResultWithLimitOnly()
        {
#if DEBUG
            // Arrange
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-1", "password", "FirstName", "LastName");
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-2", "password", "FirstName", "LastName");
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login-3", "password", "FirstName", "LastName");

            int? limit = 2;

            // Act
            var result = await _finder.List(null, null, limit);

            // Assert
            result.Should().HaveCount(2);
#endif
        }

        [TestMethod]
        public async Task ListShouldReturnEmptyResult()
        {
#if DEBUG
            // Act
            var result = await _finder.List(null, null, null);

            // Assert
            result.Should().BeEmpty();
#endif
        }

        [DataTestMethod]
        [DataRow("login")]
        [DataRow("LogiN")]
        [DataRow("LOGIN")]
        public async Task ExistsShouldSucceed(string login)
        {
#if DEBUG
            // Arrange
            RepositoryHelper.CreateUser(Guid.NewGuid(), "login", "password", "FirstName", "LastName");

            // Act
            var result = await _finder.Exists(login);

            // Assert
            result.Should().BeTrue();
#endif
        }
    }
}
