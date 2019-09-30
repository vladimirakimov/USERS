using FluentAssertions;
using ITG.Brix.Users.API.Context.Services.Responses.Impl;
using ITG.Brix.Users.API.Context.Services.Responses.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Responses
{
    [TestClass]
    public class ApiResponseTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Arrange
            var errorMapper = new Mock<IErrorMapper>().Object;
            var httpStatusCodeMapper = new Mock<IHttpStatusCodeMapper>().Object;

            // Act
            var obj = new ApiResponse(errorMapper, httpStatusCodeMapper);

            // Assert
            obj.Should().NotBeNull();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenErrorMapperIsNull()
        {
            // Arrange
            IErrorMapper errorMapper = null;
            var httpStatusCodeMapper = new Mock<IHttpStatusCodeMapper>().Object;

            // Act
            Action ctor = () => { new ApiResponse(errorMapper, httpStatusCodeMapper); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void ConstructorShouldFailWhenHttpStatusCodeMapperIsNull()
        {
            // Arrange
            var errorMapper = new Mock<IErrorMapper>().Object;
            IHttpStatusCodeMapper httpStatusCodeMapper = null;

            // Act
            Action ctor = () => { new ApiResponse(errorMapper, httpStatusCodeMapper); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }
    }
}
