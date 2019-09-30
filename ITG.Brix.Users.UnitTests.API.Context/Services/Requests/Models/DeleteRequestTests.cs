using FluentAssertions;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Requests.Models
{
    [TestClass]
    public class DeleteRequestTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var route = new DeleteFromRoute();
            var query = new DeleteFromQuery();
            var header = new DeleteFromHeader();


            // Act
            var obj = new DeleteRequest(route, query, header);

            // Assert
            obj.Should().NotBeNull();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenRouteIsNull()
        {
            // Arrange
            DeleteFromRoute route = null;
            var query = new DeleteFromQuery();
            var header = new DeleteFromHeader();

            // Act
            Action ctor = () => { new DeleteRequest(route, query, header); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenQueryIsNull()
        {
            // Arrange
            var route = new DeleteFromRoute();
            DeleteFromQuery query = null;
            var header = new DeleteFromHeader();

            // Act
            Action ctor = () => { new DeleteRequest(route, query, header); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenHeaderIsNull()
        {
            // Arrange
            var route = new DeleteFromRoute();
            var query = new DeleteFromQuery();
            DeleteFromHeader header = null;

            // Act
            Action ctor = () => { new DeleteRequest(route, query, header); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void MemberRouteIdShouldHaveCorrectValue()
        {
            // Arrange
            var routeId = Guid.NewGuid().ToString();
            var route = new DeleteFromRoute() { Id = routeId };
            var query = new DeleteFromQuery();
            var header = new DeleteFromHeader();

            // Act
            var obj = new DeleteRequest(route, query, header);

            // Assert
            obj.RouteId.Should().Be(routeId);
        }

        [TestMethod]
        public void MemberQueryApiVersionShouldHaveCorrectValue()
        {
            // Arrange
            var apiVersion = "1.0";
            var route = new DeleteFromRoute();
            var query = new DeleteFromQuery() { ApiVersion = apiVersion };
            var header = new DeleteFromHeader();

            // Act
            var obj = new DeleteRequest(route, query, header);

            // Assert
            obj.QueryApiVersion.Should().Be(apiVersion);
        }

        [TestMethod]
        public void MemberHeaderIfMatchShouldHaveCorrectValue()
        {
            // Arrange
            var ifMatch = "287687687";
            var route = new DeleteFromRoute();
            var query = new DeleteFromQuery();
            var header = new DeleteFromHeader() { IfMatch = ifMatch };

            // Act
            var obj = new DeleteRequest(route, query, header);

            // Assert
            obj.HeaderIfMatch.Should().Be(ifMatch);
        }
    }
}
