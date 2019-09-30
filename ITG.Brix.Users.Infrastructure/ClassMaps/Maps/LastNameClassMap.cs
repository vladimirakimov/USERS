using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps.Bases;
using MongoDB.Bson.Serialization;

namespace ITG.Brix.Users.Infrastructure.ClassMaps.Maps
{
    public class LastNameClassMap : DomainClassMap<LastName>
    {
        public override void Map(BsonClassMap<LastName> cm)
        {
            cm.MapCreator(x => new LastName(x.Value));
        }
    }
}
