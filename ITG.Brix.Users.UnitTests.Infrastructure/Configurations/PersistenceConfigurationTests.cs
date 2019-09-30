using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Configurations;
using ITG.Brix.Users.Infrastructure.Configurations.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Configurations
{
    [TestClass]
    public class PersistenceConfigurationTests
    {
        [TestMethod]
        public void CtorShouldSucceed()
        {
            // Arrange
            var connectionString = "connectionStringValue";

            // Act
            IPersistenceConfiguration result = new PersistenceConfiguration(connectionString);

            // Assert
            result.ConnectionString.Should().Be(connectionString);
            result.Database.Should().Be("Brix-Users");
            result.Collection.Should().Be("Users");
        }

        [TestMethod]
        public void CtorShouldFail()
        {
            // Arrange
            string connectionString = null;

            // Act
            Action ctor = () => { new PersistenceConfiguration(connectionString); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }
    }
}
