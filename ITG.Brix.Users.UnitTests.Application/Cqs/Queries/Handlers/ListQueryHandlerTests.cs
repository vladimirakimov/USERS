using AutoMapper;
using FluentAssertions;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Queries;
using ITG.Brix.Users.Application.Cqs.Queries.Handlers;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using ITG.Brix.Users.Application.Resources;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Exceptions;
using ITG.Brix.Users.Infrastructure.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.UnitTests.Application.Cqs.Queries.Handlers
{
    [TestClass]
    public class ListQueryHandlerTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var mapper = new Mock<IMapper>().Object;
            var userFinder = new Mock<IUserFinder>().Object;
            var odataProvider = new Mock<IOdataProvider>().Object;

            // Act
            Action ctor = () => { new ListQueryHandler(mapper, userFinder, odataProvider); };

            // Assert
            ctor.Should().NotThrow();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenMapperIsNull()
        {
            // Arrange
            IMapper mapper = null;
            var userFinder = new Mock<IUserFinder>().Object;
            var odataProvider = new Mock<IOdataProvider>().Object;

            // Act
            Action ctor = () => { new ListQueryHandler(mapper, userFinder, odataProvider); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenUserFinderIsNull()
        {
            // Arrange
            var mapper = new Mock<IMapper>().Object;
            IUserFinder userFinder = null;
            var odataProvider = new Mock<IOdataProvider>().Object;

            // Act
            Action ctor = () => { new ListQueryHandler(mapper, userFinder, odataProvider); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenOdataProviderIsNull()
        {
            // Arrange
            var mapper = new Mock<IMapper>().Object;
            var userFinder = new Mock<IUserFinder>().Object;
            IOdataProvider odataProvider = null;

            // Act
            Action ctor = () => { new ListQueryHandler(mapper, userFinder, odataProvider); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public async Task HandleShouldReturnOk()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<UserModel>>(It.IsAny<object>())).Returns(new List<UserModel>());
            var mapper = mapperMock.Object;

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.List(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(Task.FromResult(new List<User>() as IEnumerable<User>));
            var userFinder = userFinderMock.Object;

            var odataProviderMock = new Mock<IOdataProvider>();
            odataProviderMock.Setup(x => x.GetFilterPredicate(It.IsAny<string>())).Returns((Expression<Func<User, bool>>)null);
            var odataProvider = odataProviderMock.Object;

            var query = new ListQuery(null, null, null);

            var handler = new ListQueryHandler(mapper, userFinder, odataProvider);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeFalse();
            result.Should().BeOfType(typeof(Result<UsersModel>));
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenFilterOdataErrorOccurs()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<UserModel>>(It.IsAny<object>())).Returns(new List<UserModel>());
            var mapper = mapperMock.Object;

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.List(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(Task.FromResult(new List<User>() as IEnumerable<User>));
            var userFinder = userFinderMock.Object;

            var odataProviderMock = new Mock<IOdataProvider>();
            odataProviderMock.Setup(x => x.GetFilterPredicate(It.IsAny<string>())).Throws<FilterODataException>();
            var odataProvider = odataProviderMock.Object;

            var query = new ListQuery(null, null, null);

            var handler = new ListQueryHandler(mapper, userFinder, odataProvider);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);


            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Code == HandlerFaultCode.InvalidQueryFilter.Name &&
                                                      x.Message == HandlerFailures.InvalidQueryFilter &&
                                                      x.Target == "$filter");
        }

        [TestMethod]
        public async Task HandleShouldReturnFailWhenDatabaseSpecificErrorOccurs()
        {
            // Arrange
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<IEnumerable<UserModel>>(It.IsAny<object>())).Returns(new List<UserModel>());
            var mapper = mapperMock.Object;

            var userFinderMock = new Mock<IUserFinder>();
            userFinderMock.Setup(x => x.List(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<int?>(), It.IsAny<int?>())).Throws<SomeDatabaseSpecificException>();
            var userFinder = userFinderMock.Object;

            var odataProviderMock = new Mock<IOdataProvider>();
            odataProviderMock.Setup(x => x.GetFilterPredicate(It.IsAny<string>())).Returns((Expression<Func<User, bool>>)null);
            var odataProvider = odataProviderMock.Object;

            var query = new ListQuery(null, null, null);

            var handler = new ListQueryHandler(mapper, userFinder, odataProvider);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);


            // Assert
            result.IsFailure.Should().BeTrue();
            result.Failures.Should().OnlyContain(x => x.Message == CustomFailures.ListUserFailure);
        }

        public class SomeDatabaseSpecificException : Exception { }
    }
}
