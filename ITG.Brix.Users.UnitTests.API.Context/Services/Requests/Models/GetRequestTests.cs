using FluentAssertions;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Requests.Models
{
    [TestClass]
    public class GetRequestTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var route = new GetFromRoute();
            var query = new GetFromQuery();


            // Act
            var obj = new GetRequest(route, query);

            // Assert
            obj.Should().NotBeNull();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenRouteIsNull()
        {
            // Arrange
            GetFromRoute route = null;
            var query = new GetFromQuery();

            // Act
            Action ctor = () => { new GetRequest(route, query); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenQueryIsNull()
        {
            // Arrange
            var route = new GetFromRoute();
            GetFromQuery query = null;

            // Act
            Action ctor = () => { new GetRequest(route, query); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void MemberRouteIdShouldHaveCorrectValue()
        {
            // Arrange
            var routeId = Guid.NewGuid().ToString();
            var route = new GetFromRoute() { Id = routeId };
            var query = new GetFromQuery();

            // Act
            var obj = new GetRequest(route, query);

            // Assert
            obj.RouteId.Should().Be(routeId);
        }

        [TestMethod]
        public void MemberQueryApiVersionShouldHaveCorrectValue()
        {
            // Arrange
            var apiVersion = "1.0";
            var route = new GetFromRoute();
            var query = new GetFromQuery() { ApiVersion = apiVersion };

            // Act
            var obj = new GetRequest(route, query);

            // Assert
            obj.QueryApiVersion.Should().Be(apiVersion);
        }
    }
}
