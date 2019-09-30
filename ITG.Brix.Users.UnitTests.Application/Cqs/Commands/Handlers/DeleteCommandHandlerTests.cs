using FluentAssertions;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Commands;
using ITG.Brix.Users.Application.Cqs.Commands.Handlers;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Application.Services;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Commands.Handlers
{
    [TestClass]
    public class DeleteCommandHandlerTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;
            var userRepository = new Mock<IUserRepository>().Object;


            // Act
            Action ctor = () => { new DeleteCommandHandler(userRepository, publishEventsService); };

            // Assert
            ctor.Should().NotThrow();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenUserRepositoryIsNull()
        {
            // Arrange
            var publishEventsService = new Mock<IPublishIntegrationEventsService>().Object;
            IUserRepository userRepository = null;

            // Act
            Action ctor = () => { new DeleteCommandHandler(userRepository, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenPublishEventsServiceIsNull()
        {
            // Arrange
            IPublishIntegrationEventsService publishEventsService = null;
            var userRepository = new Mock<IUserRepository>().Object;

            // Act
            Action ctor = () => { new DeleteCommandHandler(userRepository, publishEventsService); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task HandleShouldReturnOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            var version = 1;

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Delete(id, version)).Returns(Task.CompletedTask);
            var userRepository = userRepositoryMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserCreated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new DeleteCommand(id, version);

            var handler = new DeleteCommandHandler(userRepository, publishIntegrationEventsService);

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

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Delete(id, version)).Throws<EntityNotFoundDbException>();
            var userRepository = userRepositoryMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserCreated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new DeleteCommand(id, version);

            var handler = new DeleteCommandHandler(userRepository, publishIntegrationEventsService);

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

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Delete(id, version)).Throws<EntityVersionDbException>();
            var userRepository = userRepositoryMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserCreated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new DeleteCommand(id, version);

            var handler = new DeleteCommandHandler(userRepository, publishIntegrationEventsService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Code == HandlerFaultCode.NotMet.Name &&
                                                      x.Message == HandlerFailures.NotMet &&
                                                      x.Target == "version");
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenDatabaseSpecificErrorOccurs()
        {
            var id = Guid.NewGuid();
            var version = 1;

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(x => x.Delete(id, version)).Throws<SomeDatabaseSpecificException>();
            var userRepository = userRepositoryMock.Object;

            var publishIntegrationEventsServiceMock = new Mock<IPublishIntegrationEventsService>();
            publishIntegrationEventsServiceMock.Setup(x => x.PublishUserCreated(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);
            var publishIntegrationEventsService = publishIntegrationEventsServiceMock.Object;

            var command = new DeleteCommand(id, version);

            var handler = new DeleteCommandHandler(userRepository, publishIntegrationEventsService);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Message == CustomFailures.DeleteUserFailure);
        }

        public class SomeDatabaseSpecificException : Exception { }
    }
}
