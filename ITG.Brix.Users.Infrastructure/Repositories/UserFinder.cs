using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Configurations;
using ITG.Brix.Users.Infrastructure.Constants;
using ITG.Brix.Users.Infrastructure.Exceptions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Infrastructure.Repositories
{
    public class UserFinder : IUserFinder
    {
        private readonly IMongoCollection<User> _collection;

        public UserFinder(IPersistenceContext persistenceContext)
        {
            if (persistenceContext == null)
            {
                throw new ArgumentNullException(nameof(persistenceContext));
            }

            _collection = persistenceContext.Database.GetCollection<User>(Consts.Collections.Users);
        }

        public async Task<IEnumerable<User>> List(Expression<Func<User, bool>> filter, int? skip, int? limit)
        {
            IFindFluent<User, User> fluent = null;
            if (filter == null)
            {
                var filterEmpty = Builders<User>.Filter.Empty;
                fluent = _collection.Find(filterEmpty);
            }
            else
            {
                fluent = _collection.Find(filter);
            }

            fluent = fluent.Skip(skip).Limit(limit);

            return await fluent.ToListAsync();
        }

        public async Task<User> Get(Guid id)
        {
            try
            {
                var findById = await _collection.FindAsync(doc => doc.Id == id);
                var user = findById.FirstOrDefault();
                if (user == null)
                {
                    throw new EntityNotFoundDbException();
                }
                return user;
            }
            catch (MongoCommandException ex)
            {
                Debug.WriteLine(ex);
                throw new GenericDbException(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public async Task<User> Get(string login)
        {
            try
            {
                var findByLogin = await _collection.FindAsync(x => x.Login.Value == login);
                var user = findByLogin.FirstOrDefault();
                if (user == null)
                {
                    throw new EntityNotFoundDbException();
                }
                return user;
            }
            catch (MongoCommandException ex)
            {
                Debug.WriteLine(ex);
                throw new GenericDbException(ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public async Task<bool> Exists(string login)
        {
            IMongoQueryable<User> query = _collection.AsQueryable();
            query = query.Where(u => u.Login.Value.ToLower() == login.ToLowerInvariant());
            var result = query.Any();
            return await Task.FromResult(result);
        }
    }
}
