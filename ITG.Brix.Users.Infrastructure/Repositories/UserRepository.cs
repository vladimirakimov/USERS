using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.Configurations;
using ITG.Brix.Users.Infrastructure.Constants;
using ITG.Brix.Users.Infrastructure.Exceptions;
using ITG.Brix.Users.Infrastructure.Extensions;
using MongoDB.Driver;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IPersistenceContext persistenceContext)
        {
            if (persistenceContext == null)
            {
                throw new ArgumentNullException(nameof(persistenceContext));
            }

            _collection = persistenceContext.Database.GetCollection<User>(Consts.Collections.Users);
        }

        public async Task Create(User user)
        {
            try
            {
                await _collection.InsertOneAsync(user);
            }
            catch (MongoWriteException ex)
            {
                if (ex.IsUniqueViolation())
                {
                    throw new UniqueKeyException(ex);
                }
                throw new GenericDbException(ex);
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

        public async Task Update(User user)
        {
            try
            {
                await _collection.ReplaceOneAsync(doc => doc.Id == user.Id, user);
            }
            catch (MongoWriteException ex)
            {
                if (ex.IsUniqueViolation())
                {
                    throw new UniqueKeyException(ex);
                }
                throw new GenericDbException(ex);
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

        public async Task Delete(Guid id, int version)
        {
            try
            {
                var findById = await _collection.FindAsync(doc => doc.Id == id);
                var user = findById.FirstOrDefault();
                if (user == null)
                {
                    throw new EntityNotFoundDbException();
                }

                var result = await _collection.DeleteOneAsync(doc => doc.Id == id && doc.Version == version);
                if (result.DeletedCount == 0)
                {
                    throw new EntityVersionDbException();
                }
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
    }
}
