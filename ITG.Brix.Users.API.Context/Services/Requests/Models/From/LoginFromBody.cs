using System.Runtime.Serialization;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models.From
{
    public class LoginFromBody
    {
        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}
