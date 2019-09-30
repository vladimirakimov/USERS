using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps.Bases;
using MongoDB.Bson.Serialization;

namespace ITG.Brix.Users.Infrastructure.ClassMaps.Maps
{
    public class UserClassMap : DomainClassMap<User>
    {
        public override void Map(BsonClassMap<User> cm)
        {
            cm.AutoMap();
        }
    }
}
