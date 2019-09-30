using FluentAssertions;
using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;

namespace ITG.Brix.Users.IntegrationTests.Infrastructure.ClassMaps
{
    [TestClass]
    public class UserClassMapTests
    {
        [ClassInitialize()]
        public static void ClassInitialize(TestContext testContext)
        {
            ClassMapsRegistrator.RegisterMaps();
        }

        [TestMethod]
        public void UserToAndFromBsonShouldSucceed()
        {
            // Arrange
            var id = Guid.NewGuid();
            var loginValue = "Login";
            var password = "Password";
            var login = new Login(loginValue);
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);
            var user = new User(id, login, password, fullName);

            // Act
            var bson = user.ToBson();
            var rehydrated = BsonSerializer.Deserialize<User>(bson);

            // Assert
            rehydrated.Should().NotBeNull();
            rehydrated.Id.Should().Be(id);
            rehydrated.Login.Should().Be(login);
            rehydrated.Password.Should().Be(password);
            rehydrated.FullName.FirstName.Should().Be(firstName);
            rehydrated.FullName.LastName.Should().Be(lastName);
        }
    }
}
