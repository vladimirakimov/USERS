using FluentAssertions;
using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Services.Responses.Mappers.Impl;
using ITG.Brix.Users.API.Context.Services.Responses.Models.Errors;
using ITG.Brix.Users.Application.Bases;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Responses.Mappers
{
    [TestClass]
    public class ErrorMapperTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Act
            var obj = new ErrorMapper();

            // Assert
            obj.Should().NotBeNull();
        }

        [TestMethod]
        public void MapShouldReturnNull()
        {
            // Arrange
            ValidationResult validationResult = new ValidationResult();
            var errorMapper = new ErrorMapper();

            // Act
            var result = errorMapper.Map(validationResult);

            // Assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void MapShouldReturnResponseError()
        {
            // Arrange
            var errorCode = "code";
            var errorMessage = "message";
            var errorTarget = "target";
            ValidationResult validationResult = new ValidationResult();
            validationResult.ServiceError = ServiceError.ResourceNotFound;
            validationResult.Errors = new List<Failure>() { new Failure() { Code = errorCode, Message = errorMessage, Target = errorTarget } };
            var errorMapper = new ErrorMapper();

            // Act
            var result = errorMapper.Map(validationResult);

            // Assert
            result.Should().BeOfType<ResponseError>();
            result.Error.Code.Should().Be(ServiceError.ResourceNotFound.Code);
            result.Error.Message.Should().Be(ServiceError.ResourceNotFound.Message);
            result.Error.Details[0].Code.Should().Be(errorCode);
            result.Error.Details[0].Message.Should().Be(errorMessage);
            result.Error.Details[0].Target.Should().Be(errorTarget);
        }
    }
}
