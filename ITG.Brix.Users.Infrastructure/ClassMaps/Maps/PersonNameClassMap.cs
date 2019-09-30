using ITG.Brix.Users.Domain;
using ITG.Brix.Users.Infrastructure.ClassMaps.Bases;
using MongoDB.Bson.Serialization;

namespace ITG.Brix.Users.Infrastructure.ClassMaps.Maps
{
    public class PersonNameClassMap : DomainClassMap<PersonName>
    {
        public override void Map(BsonClassMap<PersonName> cm)
        {
            cm.MapMember(x => x.Value);
            cm.AddKnownType(typeof(FirstName));
            cm.AddKnownType(typeof(LastName));
        }
    }
}
