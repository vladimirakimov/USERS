using FluentAssertions;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Requests.Models
{
    [TestClass]
    public class CreateRequestTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var query = new CreateFromQuery();
            var body = new CreateFromBody();

            // Act
            var obj = new CreateRequest(query, body);

            // Assert
            obj.Should().NotBeNull();
        }


        [TestMethod]
        public void ConstructorShouldFailWhenQueryIsNull()
        {
            // Arrange
            CreateFromQuery query = null;
            var body = new CreateFromBody();

            // Act
            Action ctor = () => { new CreateRequest(query, body); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenBodyIsNull()
        {
            // Arrange
            var query = new CreateFromQuery();
            CreateFromBody body = null;

            // Act
            Action ctor = () => { new CreateRequest(query, body); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void MemberQueryApiVersionShouldHaveCorrectValue()
        {
            // Arrange
            var apiVersion = "1.0";
            var query = new CreateFromQuery() { ApiVersion = apiVersion };
            var body = new CreateFromBody();

            // Act
            var obj = new CreateRequest(query, body);

            // Assert
            obj.QueryApiVersion.Should().Be(apiVersion);
        }

        [DataTestMethod]
        [DataRow("Login", "Password", "FirstName", "LastName")]
        [DataRow("", "", "", "")]
        [DataRow(null, null, null, null)]
        public void MembersBodyShouldHaveCorrectValue(string login,
                                                      string password,
                                                      string firstName,
                                                      string lastName)
        {
            // Arrange
            var query = new CreateFromQuery();
            var body = new CreateFromBody()
            {
                Login = login,
                Password = password,
                FirstName = firstName,
                LastName = lastName
            };

            // Act
            var obj = new CreateRequest(query, body);

            // Assert
            obj.BodyLogin.Should().Be(login);
            obj.BodyPassword.Should().Be(password);
            obj.BodyFirstName.Should().Be(firstName);
            obj.BodyLastName.Should().Be(lastName);
        }

    }
}
