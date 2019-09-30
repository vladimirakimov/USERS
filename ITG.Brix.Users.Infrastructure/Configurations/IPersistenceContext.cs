using MongoDB.Driver;

namespace ITG.Brix.Users.Infrastructure.Configurations
{
    public interface IPersistenceContext
    {
        IMongoDatabase Database { get; }
    }
}
