using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Providers;
using ITG.Brix.Users.Infrastructure.Providers.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Providers
{
    [TestClass]
    public class PasswordProviderTests
    {
        [TestMethod]
        public void HashShouldSucceed()
        {
            // Arrange
            IPasswordProvider passwordProvider = new PasswordProvider();
            var password = "secret";

            // Act
            var result = passwordProvider.Hash(password);

            // Assert
            result.Should().NotBeNull();
        }

        [TestMethod]
        public void VerifyShouldReturnTrue()
        {
            // Arrange
            IPasswordProvider passwordProvider = new PasswordProvider();
            var password = "secret";
            var passwordHashed = passwordProvider.Hash(password);

            // Act
            var result = passwordProvider.Verify(password, passwordHashed);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public void VerifyShouldReturnFalse()
        {
            // Arrange
            IPasswordProvider passwordProvider = new PasswordProvider();
            var password = "secret";
            var passwordOther = "another-secret";
            var passwordHashed = passwordProvider.Hash(password);

            // Act
            var result = passwordProvider.Verify(passwordOther, passwordHashed);

            // Assert
            result.Should().BeFalse();
        }
    }
}
