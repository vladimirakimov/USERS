using FluentAssertions;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Commands;
using ITG.Brix.Users.Application.Cqs.Commands.Handlers;
using ITG.Brix.Users.Application.DataTypes;
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
    public class UpdateCommandHandlerTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var userFinder = new Mock<IUserFinder>().Object;
            var userRepository = new Mock<IUserRepository>().Object;
            var versionProvider = new Mock<IVersionProvider>().Object;
            var passwordProvider = new Mock<IPasswordProvider>().Object;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new UpdateCommandHandler(userFinder, userRepository, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().NotThrow();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenUserFinderIsNull()
        {
            // Arrange
            IUserFinder userFinder = null;
            var userRepository = new Mock<IUserRepository>().Object;
            var versionProvider = new Mock<IVersionProvider>().Object;
            var passwordProvider = new Mock<IPasswordProvider>().Object;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new UpdateCommandHandler(userFinder, userRepository, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenUserRepositoryIsNull()
        {
            // Arrange
            var userFinder = new Mock<IUserFinder>().Object;
            IUserRepository userRepository = null;
            var versionProvider = new Mock<IVersionProvider>().Object;
            var passwordProvider = new Mock<IPasswordProvider>().Object;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new UpdateCommandHandler(userFinder, userRepository, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenVersionProviderIsNull()
        {
            // Arrange
            var userFinder = new Mock<IUserFinder>().Object;
            var userRepository = new Mock<IUserRepository>().Object;
            IVersionProvider versionProvider = null;
            var passwordProvider = new Mock<IPasswordProvider>().Object;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new UpdateCommandHandler(userFinder, userRepository, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenPasswordProviderIsNull()
        {
            // Arrange
            var userFinder = new Mock<IUserFinder>().Object;
            var userRepository = new Mock<IUserRepository>().Object;
            var versionProvider = new Mock<IVersionProvider>().Object;
            IPasswordProvider passwordProvider = null;
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;

            // Act
            Action ctor = () => { new UpdateCommandHandler(userFinder, userRepository, versionProvider, passwordProvider, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task HandleShouldReturnOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var version = 1;

            Optional<string> login = new Optional<string>("Login");
            Optional<string> password = new Optional<string>("Password$my");
            Optional<string> firstName = new Optional<string>("FirstName");
            Optional<string> lastName = new Optional<string>("LastName");

            var fullName = new FullName(new FirstName(firstName.Value), new LastName(lastName.Value));
            var userFromDatabase = new User(id, new Login("Login"), "Password$my", fullName) { Version = 1 };

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.Get(id)).Returns(Task.FromResult(userFromDatabase));
            var userFinder = userFinderMock.Object;

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Returns(Task.CompletedTask);
            var userRepository = userRepositoryMock.Object;

            var versionProviderMock = new Mock<IVersionProvider>();
            versionProviderMock.Setup(x => x.Generate()).Returns(version);
            var versionProvider = versionProviderMock.Object;

            var passwordProviderMock = new Mock<IPasswordProvider>();
            passwordProviderMock.Setup(x => x.Hash(It.IsAny<string>())).Returns("hashedPassword");
            var passwordProvider = passwordProviderMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserUpdated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new UpdateCommand(id,
                                            login,
                                            password,
                                            firstName,
                                            lastName,
                                            version);

            var handler = new UpdateCommandHandler(userFinder, userRepository, versionProvider, passwordProvider, publishIntegrationEventsService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeFalse();
            result.Should().BeOfType(typeof(Result));
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var version = 1;

            Optional<string> login = new Optional<string>("Login");
            Optional<string> password = new Optional<string>("Password$my");
            Optional<string> firstName = new Optional<string>("FirstName");
            Optional<string> lastName = new Optional<string>("LastName");

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.Get(id)).Throws<EntityNotFoundDbException>();
            var userFinder = userFinderMock.Object;

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Returns(Task.CompletedTask);
            var userRepository = userRepositoryMock.Object;

            var versionProviderMock = new Mock<IVersionProvider>();
            versionProviderMock.Setup(x => x.Generate()).Returns(version);
            var versionProvider = versionProviderMock.Object;

            var passwordProviderMock = new Mock<IPasswordProvider>();
            passwordProviderMock.Setup(x => x.Hash(It.IsAny<string>())).Returns("hashedPassword");
            var passwordProvider = passwordProviderMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserCreated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new UpdateCommand(id,
                                            login,
                                            password,
                                            firstName,
                                            lastName,
                                            version);

            var handler = new UpdateCommandHandler(userFinder, userRepository, versionProvider, passwordProvider, publishIntegrationEventsService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Code == HandlerFaultCode.NotFound.Name &&
                                                      x.Message == HandlerFailures.NotFound &&
                                                      x.Target == "id");
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenOutdatedVersion()
        {
            // Arrange
            var id = Guid.NewGuid();
            var version = 1;

            Optional<string> login = new Optional<string>("Login");
            Optional<string> password = new Optional<string>("Password$my");
            Optional<string> firstName = new Optional<string>("FirstName");
            Optional<string> lastName = new Optional<string>("LastName");

            var fullName = new FullName(new FirstName(firstName.Value), new LastName(lastName.Value));
            var userFromDatabase = new User(id, new Login("Login"), "Password$my", fullName) { Version = 2 };

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.Get(id)).Returns(Task.FromResult(userFromDatabase));
            var userFinder = userFinderMock.Object;

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Update(It.IsAny<User>())).Returns(Task.CompletedTask);
            var userRepository = userRepositoryMock.Object;

            var versionProviderMock = new Mock<IVersionProvider>();
            versionProviderMock.Setup(x => x.Generate()).Returns(version);
            var versionProvider = versionProviderMock.Object;

            var passwordProviderMock = new Mock<IPasswordProvider>();
            passwordProviderMock.Setup(x => x.Hash(It.IsAny<string>())).Returns("hashedPassword");
            var passwordProvider = passwordProviderMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserCreated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new UpdateCommand(id,
                                            login,
                                            password,
                                            firstName,
                                            lastName,
                                            version);

            var handler = new UpdateCommandHandler(userFinder, userRepository, versionProvider, passwordProvider, publishIntegrationEventsService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Code == HandlerFaultCode.NotMet.Name &&
                                          x.Message == HandlerFailures.NotMet &&
                                          x.Target == "version");
        }
    }
}
