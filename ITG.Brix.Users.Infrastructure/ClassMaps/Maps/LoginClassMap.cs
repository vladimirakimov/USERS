using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps.Bases;
using MongoDB.Bson.Serialization;

namespace ITG.Brix.Users.Infrastructure.ClassMaps.Maps
{
    public class LoginClassMap : DomainClassMap<Login>
    {
        public override void Map(BsonClassMap<Login> cm)
        {
            cm.AutoMap();
        }
    }
}
