using System.Runtime.Serialization;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models.From
{
    public class CreateFromBody
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
    }
}
