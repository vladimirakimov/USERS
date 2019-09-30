using FluentAssertions;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Requests.Models
{
    [TestClass]
    public class UpdateRequestTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var route = new UpdateFromRoute();
            var query = new UpdateFromQuery();
            var header = new UpdateFromHeader();
            var body = new UpdateFromBody();

            // Act
            var obj = new UpdateRequest(route, query, header, body);

            // Assert
            obj.Should().NotBeNull();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenRouteIsNull()
        {
            // Arrange
            UpdateFromRoute route = null;
            var query = new UpdateFromQuery();
            var header = new UpdateFromHeader();
            var body = new UpdateFromBody();

            // Act
            Action ctor = () => { new UpdateRequest(route, query, header, body); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenQueryIsNull()
        {
            // Arrange
            var route = new UpdateFromRoute();
            UpdateFromQuery query = null;
            var header = new UpdateFromHeader();
            var body = new UpdateFromBody();

            // Act
            Action ctor = () => { new UpdateRequest(route, query, header, body); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenHeaderIsNull()
        {
            // Arrange
            var route = new UpdateFromRoute();
            var query = new UpdateFromQuery();
            UpdateFromHeader header = null;
            var body = new UpdateFromBody();

            // Act
            Action ctor = () => { new UpdateRequest(route, query, header, body); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenBodyIsNull()
        {
            // Arrange
            var route = new UpdateFromRoute();
            var query = new UpdateFromQuery();
            var header = new UpdateFromHeader();
            UpdateFromBody body = null;

            // Act
            Action ctor = () => { new UpdateRequest(route, query, header, body); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void MemberRouteIdShouldHaveCorrectValue()
        {
            // Arrange
            var routeId = Guid.NewGuid().ToString();
            var route = new UpdateFromRoute() { Id = routeId };
            var query = new UpdateFromQuery();
            var header = new UpdateFromHeader();
            var body = new UpdateFromBody();

            // Act
            var obj = new UpdateRequest(route, query, header, body);

            // Assert
            obj.RouteId.Should().Be(routeId);
        }

        [TestMethod]
        public void MemberQueryApiVersionShouldHaveCorrectValue()
        {
            // Arrange
            var apiVersion = "1.0";
            var route = new UpdateFromRoute();
            var query = new UpdateFromQuery() { ApiVersion = apiVersion };
            var header = new UpdateFromHeader();
            var body = new UpdateFromBody();

            // Act
            var obj = new UpdateRequest(route, query, header, body);

            // Assert
            obj.QueryApiVersion.Should().Be(apiVersion);
        }

        [TestMethod]
        public void MembersHeaderShouldHaveCorrectValue()
        {
            // Arrange
            var contentType = "application/json";
            var ifMatch = "3452353445435";

            var route = new UpdateFromRoute();
            var query = new UpdateFromQuery();
            var header = new UpdateFromHeader() { IfMatch = ifMatch, ContentType = contentType };
            var body = new UpdateFromBody();


            // Act
            var obj = new UpdateRequest(route, query, header, body);

            // Assert
            obj.HeaderContentType.Should().Be(contentType);
            obj.HeaderIfMatch.Should().Be(ifMatch);
        }


        [TestMethod]
        public void MemberBodyPatchShouldHaveCorrectValue()
        {
            // Arrange
            var patch = "json";
            var route = new UpdateFromRoute();
            var query = new UpdateFromQuery();
            var header = new UpdateFromHeader();
            var body = new UpdateFromBody()
            {
                Patch = patch
            };

            // Act
            var obj = new UpdateRequest(route, query, header, body);

            // Assert
            obj.BodyPatch.Should().Be(patch);
        }
    }
}
