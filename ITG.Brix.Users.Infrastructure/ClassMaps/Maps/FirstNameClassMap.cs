using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps.Bases;
using MongoDB.Bson.Serialization;

namespace ITG.Brix.Users.Infrastructure.ClassMaps.Maps
{
    public class FirstNameClassMap : DomainClassMap<FirstName>
    {
        public override void Map(BsonClassMap<FirstName> cm)
        {
            cm.MapCreator(x => new FirstName(x.Value));
        }
    }
}
