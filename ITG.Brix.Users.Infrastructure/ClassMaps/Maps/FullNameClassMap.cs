using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps.Bases;
using MongoDB.Bson.Serialization;

namespace ITG.Brix.Users.Infrastructure.ClassMaps.Maps
{
    public class FullNameClassMap : DomainClassMap<FullName>
    {
        public override void Map(BsonClassMap<FullName> cm)
        {
            cm.AutoMap();
        }
    }
}
