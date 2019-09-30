using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Clusters;
using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Servers;
using System;
using System.Net;
using System.Reflection;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Extensions
{
    [TestClass]
    public class MongoWriteExceptionExtensionsTests
    {
        [TestMethod]
        public void ShouldReturnFalseIfPassedNullMongoWriteException()
        {
            // Arrange
            MongoWriteException exception = null;

            // Act
            var result = exception.IsUniqueViolation();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldReturnFalseIfPassedMongoWriteExceptionWithWriteErrorAsNull()
        {
            // Arrange
            var connectionId = new ConnectionId(new ServerId(new ClusterId(1), new DnsEndPoint("localhost", 27017)), 2);
            WriteError writeError = null;
            WriteConcernError writeConcernError = null;
            Exception innerException = new InvalidOperationException();
            var exception = new MongoWriteException(connectionId, writeError, writeConcernError, innerException);

            // Act
            var result = exception.IsUniqueViolation();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldReturnFalseIfPassedMongoWriteExceptionWithWriteError()
        {
            // Arrange
            var connectionId = new ConnectionId(new ServerId(new ClusterId(1), new DnsEndPoint("localhost", 27017)), 2);
            var innerException = new Exception("inner");
            WriteConcernError writeConcernError = null;
            var ctor = typeof(WriteError).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
            var writeError = (WriteError)ctor.Invoke(new object[] { ServerErrorCategory.Uncategorized, 1, "writeError", new BsonDocument("details", "writeError") });
            var exception = new MongoWriteException(connectionId, writeError, writeConcernError, innerException);

            // Act
            var result = exception.IsUniqueViolation();

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public void ShouldReturnTrueIfPassedMongoWriteExceptionWithWriteError()
        {
            // Arrange
            var connectionId = new ConnectionId(new ServerId(new ClusterId(1), new DnsEndPoint("localhost", 27017)), 2);
            var innerException = new Exception("inner");
            WriteConcernError writeConcernError = null;
            var ctor = typeof(WriteError).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)[0];
            var writeError = (WriteError)ctor.Invoke(new object[] { ServerErrorCategory.Uncategorized, MongoUniqueViolationCode, "writeError", new BsonDocument("details", "writeError") });
            var exception = new MongoWriteException(connectionId, writeError, writeConcernError, innerException);

            // Act
            var result = exception.IsUniqueViolation();

            // Assert
            result.Should().BeTrue();
        }

        private static readonly int MongoUniqueViolationCode = 11000;
    }


}
