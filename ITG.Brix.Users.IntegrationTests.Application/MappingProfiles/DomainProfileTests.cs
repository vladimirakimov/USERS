using AutoMapper;
using FluentAssertions;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using ITG.Brix.Users.Application.MappingProfiles;
using ITG.Brix.Users.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.IntegrationTests.Application.MappingProfiles
{
    [TestClass]
    public class DomainProfileTests
    {
        [AssemblyInitialize()]
        public static void ClassInit(TestContext context)
        {
            Mapper.Initialize(m => m.AddProfile<DomainProfile>());
        }

        [TestMethod]
        public void AutoMapperConfigurationShouldBeValid()
        {
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void UserToUserModelShouldMapCorrectly()
        {
            var login = new Login("Login");
            var firstName = new FirstName("FirstName");
            var lastName = new LastName("LastName");
            var fullName = new FullName(firstName, lastName);
            var user = new User(Guid.NewGuid(), login, "Password", fullName);
            var userModel = Mapper.Map<UserModel>(user);
            userModel.Should().NotBeNull();
        }
    }
}
