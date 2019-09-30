using FluentAssertions;
using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Services.Responses.Mappers.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace ITG.Brix.Users.UnitTests.API.Context.Services.Responses.Mappers
{
    [TestClass]
    public class HttpStatusCodeMapperTests
    {
        [TestMethod]
        public void ConstructorShouldSucceed()
        {
            // Act
            var obj = new HttpStatusCodeMapper();

            // Assert
            obj.Should().NotBeNull();
        }


        [TestMethod]
        public void MapShouldReturnUnsupportedMediaType()
        {
            // Arrange
            ValidationResult validationResult = new ValidationResult();
            validationResult.ServiceError = ServiceError.UnsupportedMediaType;
            var errorMapper = new HttpStatusCodeMapper();

            // Act
            var result = errorMapper.Map(validationResult);

            // Assert
            result.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [TestMethod]
        public void MapShouldReturnBadRequest()
        {
            // Arrange
            ValidationResult validationResult = new ValidationResult();
            validationResult.ServiceError = ServiceError.InvalidInput;
            var errorMapper = new HttpStatusCodeMapper();

            // Act
            var result = errorMapper.Map(validationResult);

            // Assert
            result.Should().Be(HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void MapShouldReturnNotFound()
        {
            // Arrange
            ValidationResult validationResult = new ValidationResult();
            validationResult.ServiceError = ServiceError.ResourceNotFound;
            var errorMapper = new HttpStatusCodeMapper();

            // Act
            var result = errorMapper.Map(validationResult);

            // Assert
            result.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public void MapShouldReturnConflict()
        {
            // Arrange
            ValidationResult validationResult = new ValidationResult();
            validationResult.ServiceError = ServiceError.ResourceAlreadyExists;
            var errorMapper = new HttpStatusCodeMapper();

            // Act
            var result = errorMapper.Map(validationResult);

            // Assert
            result.Should().Be(HttpStatusCode.Conflict);
        }

        [TestMethod]
        public void MapShouldReturnPreconditionFailed()
        {
            // Arrange
            ValidationResult validationResult = new ValidationResult();
            validationResult.ServiceError = ServiceError.ConditionNotMet;
            var errorMapper = new HttpStatusCodeMapper();

            // Act
            var result = errorMapper.Map(validationResult);

            // Assert
            result.Should().Be(HttpStatusCode.PreconditionFailed);
        }

        [TestMethod]
        public void MapShouldReturnInternalServerError()
        {
            // Arrange
            ValidationResult validationResult = new ValidationResult();
            validationResult.ServiceError = ServiceError.None;// assumed "UserDefinedExceptionError"
            var errorMapper = new HttpStatusCodeMapper();

            // Act
            var result = errorMapper.Map(validationResult);

            // Assert
            result.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
