using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Configurations;
using ITG.Brix.Users.Infrastructure.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Repositories
{
    [TestClass]
    public class UserRepositoryTests
    {
        [TestMethod]
        public void ConstructorShouldFailWhenPersistenceContextNull()
        {
            // Arrange
            IPersistenceContext persistenceContext = null;

            // Act
            Action ctor = () => { new UserRepository(persistenceContext); };

            // Assert
            ctor.Should().Throw<ArgumentNullException>();
        }
    }
}
