using FluentAssertions;
using ITG.Brix.Users.Infrastructure.Providers;
using ITG.Brix.Users.Infrastructure.Providers.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace ITG.Brix.Users.UnitTests.Infrastructure.Providers
{
    [TestClass]
    public class OdataProviderTests
    {
        [DataTestMethod]
        [FilterTransformTestData]
        public void FilterTransformShouldReturnCorrectResult(string filter, Dictionary<string, string> replacements, string expected)
        {
            // Arrange
            IOdataProvider odataProvider = new OdataProvider();

            // Act
            var result = odataProvider.FilterTransform(filter, replacements);

            // Assert
            result.Should().Be(expected);
        }
    }

    internal class FilterTransformTestDataAttribute : Attribute, ITestDataSource
    {
        public IEnumerable<object[]> GetData(MethodInfo methodInfo)
        {
            return new List<object[]> {
            new object[] { "firstName eq 'FirstName'", new Dictionary<string, string>() { { "firstName", "FullName/First" } }, "FullName/First eq 'FirstName'" },
            new object[] { "login eq 'Hero'", new Dictionary<string, string>() { { "login", "Login/Value" } }, "Login/Value eq 'Hero'" },
            new object[] { null, new Dictionary<string, string>() { { "firstName", "FullName/First" } }, null },
            new object[] { "", new Dictionary<string, string>() { { "firstName", "FullName/First" } }, null },
            new object[] { "  ", new Dictionary<string, string>() { { "firstName", "FullName/First" } }, null },
        };
        }

        public string GetDisplayName(MethodInfo methodInfo, object[] data)
        {
            if (data != null)
            {
                return string.Format(CultureInfo.CurrentCulture, "{0} ({1})", methodInfo.Name, string.Join(",", data));
            }
            return null;
        }
    }
}
