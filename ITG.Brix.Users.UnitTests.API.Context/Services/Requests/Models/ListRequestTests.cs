using FluentAssertions;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Requests.Models
{
    [TestClass]
    public class ListRequestTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var query = new ListFromQuery();


            // Act
            var obj = new ListRequest(query);

            // Assert
            obj.Should().NotBeNull();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenQueryIsNull()
        {
            // Arrange
            ListFromQuery query = null;

            // Act
            Action ctor = () => { new ListRequest(query); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void MemberQueryApiVersionShouldHaveCorrectValue()
        {
            // Arrange
            var apiVersion = "1.0";
            var query = new ListFromQuery() { ApiVersion = apiVersion };

            // Act
            var obj = new ListRequest(query);

            // Assert
            obj.QueryApiVersion.Should().Be(apiVersion);
        }
    }
}
