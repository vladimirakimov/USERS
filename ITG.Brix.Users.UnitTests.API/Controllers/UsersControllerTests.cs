using FluentAssertions;
using ITG.Brix.Users.API.Context.Services;
using ITG.Brix.Users.API.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ITG.Brix.Users.UnitTests.API.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        [TestMethod]
        public void ConstructorShouldRegisterAllDependencies()
        {
            // Arrange
            var apiResult = new Mock<IApiResult>().Object;

            // Act
            var result = new UsersController(apiResult);

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenApiResultNull()
        {
            // Arrange
            IApiResult apiResult = null;

            // Act
            Action ctor = () => { new UsersController(apiResult); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>().WithMessage($"*{nameof(apiResult)}*");
        }
    }
}
