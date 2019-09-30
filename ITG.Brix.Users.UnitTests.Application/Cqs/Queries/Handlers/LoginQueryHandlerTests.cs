using FluentAssertions;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Queries;
using ITG.Brix.Users.Application.Cqs.Queries.Handlers;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Queries.Handlers
{
    [TestClass]
    public class LoginQueryHandlerTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var userFinder = new Mock<IUserFinder>().Object;
            var passwordProvider = new Mock<IPasswordProvider>().Object;

            // Act
            Action ctor = () => { new LoginQueryHandler(userFinder, passwordProvider); };

            // Assert
            ctor.Should().NotThrow();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenUserFinderIsNull()
        {
            // Arrange
            IUserFinder userFinder = null;
            var passwordProvider = new Mock<IPasswordProvider>().Object;

            // Act
            Action ctor = () => { new LoginQueryHandler(userFinder, passwordProvider); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenPasswordProviderIsNull()
        {
            // Arrange
            var userFinder = new Mock<IUserFinder>().Object;
            IPasswordProvider passwordProvider = null;

            // Act
            Action ctor = () => { new LoginQueryHandler(userFinder, passwordProvider); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task HandleShouldReturnOk()
        {
            // Arrange
            var id = Guid.NewGuid();

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(new User(id, new Login("login"), "password", new FullName(new FirstName("FirstName"), new LastName("FirstName")))));
            var userFinder = userFinderMock.Object;

            var passwordProviderMock = new Mock<IPasswordProvider>();
            passwordProviderMock.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var passwordProvider = passwordProviderMock.Object;

            var query = new LoginQuery(null, null);

            var handler = new LoginQueryHandler(userFinder, passwordProvider);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeFalse();
            result.Should().BeOfType(typeof(Result<AuthenticatedUserModel>));
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenWrongPassword()
        {
            // Arrange
            var id = Guid.NewGuid();

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.Get(It.IsAny<string>())).Returns(Task.FromResult(new User(id, new Login("login"), "password", new FullName(new FirstName("FirstName"), new LastName("FirstName")))));
            var userFinder = userFinderMock.Object;

            var passwordProviderMock = new Mock<IPasswordProvider>();
            passwordProviderMock.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var passwordProvider = passwordProviderMock.Object;

            var query = new LoginQuery(null, null);

            var handler = new LoginQueryHandler(userFinder, passwordProvider);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Code == HandlerFaultCode.InvalidCredentials.Name &&
                                                      x.Message == HandlerFailures.InvalidCredentials &&
                                                      x.Target == "credentials");
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenDatabaseSpecificErrorOccurs()
        {
            var id = Guid.NewGuid();

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.Get(id)).Throws<SomeDatabaseSpecificException>();
            var userFinder = userFinderMock.Object;

            var passwordProviderMock = new Mock<IPasswordProvider>();
            passwordProviderMock.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var passwordProvider = passwordProviderMock.Object;

            var query = new LoginQuery(null, null);

            var handler = new LoginQueryHandler(userFinder, passwordProvider);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Message == CustomFailures.LoginUserFailure);
        }

        public class SomeDatabaseSpecificException : Exception { }
    }
}
