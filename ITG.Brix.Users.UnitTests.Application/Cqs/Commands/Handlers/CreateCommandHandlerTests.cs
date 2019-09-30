using FluentAssertions;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Commands;
using ITG.Brix.Users.Application.Cqs.Commands.Handlers;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Application.Services;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using ITG.Brix.Users.Infrastructure.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Commands.Handlers
{
    [TestClass]
    public class CreateCommandHandlerTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>().Object;
            var identifierProvider = new Mock<IIdentifierProvider>().Object;
            var versionProvider = new Mock<IVersionProvider>().Object;
            var passwordProvider = new Mock<IPasswordProvider>().Object;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new CreateCommandHandler(userRepository, identifierProvider, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().NotThrow();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenUserRepositoryIsNull()
        {
            // Arrange
            IUserRepository userRepository = null;
            var identifierProvider = new Mock<IIdentifierProvider>().Object;
            var versionProvider = new Mock<IVersionProvider>().Object;
            var passwordProvider = new Mock<IPasswordProvider>().Object;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new CreateCommandHandler(userRepository, identifierProvider, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenIdentifierProviderIsNull()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>().Object;
            IIdentifierProvider identifierProvider = null;
            var versionProvider = new Mock<IVersionProvider>().Object;
            var passwordProvider = new Mock<IPasswordProvider>().Object;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new CreateCommandHandler(userRepository, identifierProvider, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenVersionProviderIsNull()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>().Object;
            var identifierProvider = new Mock<IIdentifierProvider>().Object;
            IVersionProvider versionProvider = null;
            var passwordProvider = new Mock<IPasswordProvider>().Object;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new CreateCommandHandler(userRepository, identifierProvider, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenPasswordProviderIsNull()
        {
            // Arrange
            var userRepository = new Mock<IUserRepository>().Object;
            var identifierProvider = new Mock<IIdentifierProvider>().Object;
            var versionProvider = new Mock<IVersionProvider>().Object;
            IPasswordProvider passwordProvider = null;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new CreateCommandHandler(userRepository, identifierProvider, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task HandleShouldReturnOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var version = 1;

            var login = "Login";
            var password = "Password$my";
            var firstName = "FirstName";
            var lastName = "LastName";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).Returns(Task.CompletedTask);
            var userRepository = userRepositoryMock.Object;

            var identifierProviderMock = new Mock<IIdentifierProvider>();
            identifierProviderMock.Setup(x => x.Generate()).Returns(id);
            var identifierProvider = identifierProviderMock.Object;

            var versionProviderMock = new Mock<IVersionProvider>();
            versionProviderMock.Setup(x => x.Generate()).Returns(version);
            var versionProvider = versionProviderMock.Object;

            var passwordProviderMock = new Mock<IPasswordProvider>();
            passwordProviderMock.Setup(x => x.Hash(It.IsAny<string>())).Returns("hashedPassword");
            var passwordProvider = passwordProviderMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserCreated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new CreateCommand(login, password, firstName, lastName);

            var handler = new CreateCommandHandler(userRepository, identifierProvider, versionProvider, passwordProvider, publishIntegrationEventsService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeFalse();
            result.Should().BeOfType(typeof(Result<Guid>));
        }

        [TestMethod]
        public async Task HandleShouldFailWhenRecordWithSameLoginAlreadyExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var version = 1;

            var login = "Login";
            var password = "Password$my";
            var firstName = "FirstName";
            var lastName = "LastName";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).Throws<UniqueKeyException>();
            var userRepository = userRepositoryMock.Object;

            var identifierProviderMock = new Mock<IIdentifierProvider>();
            identifierProviderMock.Setup(x => x.Generate()).Returns(id);
            var identifierProvider = identifierProviderMock.Object;

            var versionProviderMock = new Mock<IVersionProvider>();
            versionProviderMock.Setup(x => x.Generate()).Returns(version);
            var versionProvider = versionProviderMock.Object;

            var passwordProviderMock = new Mock<IPasswordProvider>();
            passwordProviderMock.Setup(x => x.Hash(It.IsAny<string>())).Returns("hashedPassword");
            var passwordProvider = passwordProviderMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserCreated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new CreateCommand(login, password, firstName, lastName);

            var handler = new CreateCommandHandler(userRepository, identifierProvider, versionProvider, passwordProvider, publishIntegrationEventsService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Failures.Should().OnlyContain(x => x.Code == HandlerFaultCode.Conflict.Name &&
                                                      x.Message == HandlerFailures.Conflict &&
                                                      x.Target == "login");
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenDatabaseSpecificErrorOccurs()
        {
            // Arrange
            var id = Guid.NewGuid();
            var version = 1;

            var login = "Login";
            var password = "Password$my";
            var firstName = "FirstName";
            var lastName = "LastName";

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).Throws<SomeDatabaseSpecificException>();
            var userRepository = userRepositoryMock.Object;

            var identifierProviderMock = new Mock<IIdentifierProvider>();
            identifierProviderMock.Setup(x => x.Generate()).Returns(id);
            var identifierProvider = identifierProviderMock.Object;

            var versionProviderMock = new Mock<IVersionProvider>();
            versionProviderMock.Setup(x => x.Generate()).Returns(version);
            var versionProvider = versionProviderMock.Object;

            var passwordProviderMock = new Mock<IPasswordProvider>();
            passwordProviderMock.Setup(x => x.Hash(It.IsAny<string>())).Returns("hashedPassword");
            var passwordProvider = passwordProviderMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserCreated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new CreateCommand(login, password, firstName, lastName);

            var handler = new CreateCommandHandler(userRepository, identifierProvider, versionProvider, passwordProvider, publishIntegrationEventsService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Message == CustomFailures.CreateUserFailure);
        }

        public class SomeDatabaseSpecificException : Exception { }
    }
}
