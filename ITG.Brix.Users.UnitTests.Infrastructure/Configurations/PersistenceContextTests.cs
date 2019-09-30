using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Configurations;
using ITG.Brix.Users.Infrastructure.Configurations.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Configurations
{
    [TestClass]
    public class PersistenceContextTests
    {
        [TestMethod]
        public void CtorShouldSucceed()
        {
            // Arrange
            var persistenceConfiguration = new Mock<IPersistenceConfiguration>().Object;

            // Act
            var obj = new PersistenceContext(persistenceConfiguration);

            // Assert
            obj.Should().NotBeNull();
        }

        [TestMethod]
        public void CtorShouldFail()
        {
            // Arrange
            IPersistenceConfiguration persistenceConfiguration = null;

            // Act
            Action ctor = () => { new PersistenceContext(persistenceConfiguration); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }
    }
}
