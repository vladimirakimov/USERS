using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Configurations.Impl;
using ITG.Brix.Users.Infrastructure.Repositories;
using System;
using System.Collections.Generic;

namespace ITG.Brix.Users.IntegrationTests.Infrastructure.Bases
{
    public static class RepositoryHelper
    {
        public static void CreateUser(Guid id, string loginValue, string password, string firstNameValue, string lastNameValue)
        {
            var repository = new UserRepository(new PersistenceContext(new PersistenceConfiguration(RepositoryTestsHelper.ConnectionString)));

            var login = new Login(loginValue);
            var firstName = new FirstName(firstNameValue);
            var lastName = new LastName(lastNameValue);
            var fullName = new FullName(firstName, lastName);
            var user = new User(id, login, password, fullName);

            repository.Create(user).GetAwaiter().GetResult();
        }

        public static IEnumerable<User> GetUsers()
        {
            var finder = new UserFinder(new PersistenceContext(new PersistenceConfiguration(RepositoryTestsHelper.ConnectionString)));
            var result = finder.List(null, null, null).Result;

            return result;
        }
    }
}
