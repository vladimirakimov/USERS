using FluentAssertions;
using ITG.Brix.Users.Application.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITG.Brix.Users.UnitTests.Application.Extensions
{
    [TestClass]
    public class StringExtensionsTests
    {
        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("    ", "    ")]
        [DataRow("a", "a")]
        [DataRow("Ab", "ab")]
        [DataRow("AB", "aB")]
        public void ToCamelCaseShouldSucceed(string input, string expected)
        {
            // Act
            var result = input.ToCamelCase();

            // Assert
            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("    ", false)]
        [DataRow("a", false)]
        [DataRow("Ab", true)]
        [DataRow("aB", false)]
        [DataRow("1A", false)]
        public void StartsWithCapitalLetterShouldSucceed(string input, bool expected)
        {
            // Act
            var result = input.StartsWithCapitalLetter();

            // Assert
            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow("    ", false)]
        [DataRow("a", false)]
        [DataRow("Ab", false)]
        [DataRow("aB", false)]
        [DataRow("1A", false)]
        [DataRow("1A!", true)]
        [DataRow("1A#", true)]
        [DataRow("1A?", true)]
        [DataRow("1A%", true)]
        [DataRow("1A&", true)]
        [DataRow("1A B", true)]
        public void AtLeastOneSpecialCharacterShouldSucceed(string input, bool expected)
        {
            // Act
            var result = input.AtLeastOneSpecialCharacter();

            // Assert
            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, false)]
        [DataRow("", false)]
        [DataRow(" ", false)]
        [DataRow("  ", false)]
        [DataRow(" a", false)]
        [DataRow("a ", false)]
        [DataRow(" a ", false)]
        [DataRow("Ab", true)]
        [DataRow("A b", true)]
        [DataRow("A b c", true)]
        [DataRow("A  b", false)]
        [DataRow("A  b c", false)]
        [DataRow("1A", false)]
        [DataRow("1A!", false)]
        [DataRow("1A#", false)]
        [DataRow("1A B", false)]
        [DataRow("1A B", false)]
        public void HasLettersOrSingleSpaceCharactersShouldSucceed(string input, bool expected)
        {
            // Act
            var result = input.HasLettersOrSingleSpaceCharacters();

            // Assert
            result.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow(null, null)]
        [DataRow("", null)]
        [DataRow("    ", null)]
        [DataRow("a", null)]
        [DataRow("1", 1)]
        public void ToNullableIntShouldSucceed(string input, int? expected)
        {
            // Act
            var result = input.ToNullableInt();

            // Assert
            result.Should().Be(expected);
        }

    }
}
