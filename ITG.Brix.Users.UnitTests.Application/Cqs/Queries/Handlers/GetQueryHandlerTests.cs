using AutoMapper;
using FluentAssertions;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Queries;
using ITG.Brix.Users.Application.Cqs.Queries.Handlers;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Queries.Handlers
{
    [TestClass]
    public class GetQueryHandlerTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var mapper = new Mock<IMapper>().Object;
            var userFinder = new Mock<IUserFinder>().Object;

            // Act
            Action ctor = () => { new GetQueryHandler(mapper, userFinder); };

            // Assert
            ctor.Should().NotThrow();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenMapperIsNull()
        {
            // Arrange
            IMapper mapper = null;
            var userFinder = new Mock<IUserFinder>().Object;

            // Act
            Action ctor = () => { new GetQueryHandler(mapper, userFinder); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenUserFinderIsNull()
        {
            // Arrange
            var mapper = new Mock<IMapper>().Object;
            IUserFinder userFinder = null;

            // Act
            Action ctor = () => { new GetQueryHandler(mapper, userFinder); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }


        [TestMethod]
        public async Task HandleShouldReturnOk()
        {
            // Arrange
            var id = Guid.NewGuid();

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.Get(id)).Returns(Task.FromResult(new User(id, new Login("login"), "password", new FullName(new FirstName("FirstName"), new LastName("LastName")))));
            var userFinder = userFinderMock.Object;

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<UserModel>(It.IsAny<object>())).Returns(new UserModel());
            var mapper = mapperMock.Object;

            var query = new GetQuery(id);

            var handler = new GetQueryHandler(mapper, userFinder);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeFalse();
            result.Should().BeOfType(typeof(Result<UserModel>));
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.Get(id)).Throws<EntityNotFoundDbException>();
            var userFinder = userFinderMock.Object;

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<UserModel>(It.IsAny<object>())).Returns(new UserModel());
            var mapper = mapperMock.Object;

            var query = new GetQuery(id);

            var handler = new GetQueryHandler(mapper, userFinder);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Code == HandlerFaultCode.NotFound.Name &&
                                                      x.Message == HandlerFailures.NotFound &&
                                                      x.Target == "id");
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenDatabaseSpecificErrorOccurs()
        {
            // Arrange
            var id = Guid.NewGuid();

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.Get(id)).Throws<SomeDatabaseSpecificException>();
            var userFinder = userFinderMock.Object;

            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<UserModel>(It.IsAny<object>())).Returns(new UserModel());
            var mapper = mapperMock.Object;

            var query = new GetQuery(id);

            var handler = new GetQueryHandler(mapper, userFinder);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Message == CustomFailures.GetUserFailure);
        }

        public class SomeDatabaseSpecificException : Exception { }
    }
}
