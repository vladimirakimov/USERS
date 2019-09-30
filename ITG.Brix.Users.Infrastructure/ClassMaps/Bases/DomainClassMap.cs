using MongoDB.Bson.Serialization;

namespace ITG.Brix.Users.Infrastructure.ClassMaps.Bases
{
    public abstract class DomainClassMap<T>
    {
        protected DomainClassMap()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap<T>(Map);
            }
        }

        public abstract void Map(BsonClassMap<T> cm);
    }
}
