using System.Collections.Generic;

namespace ITG.Brix.Users.Application.Cqs.Queries.Models
{
    public class UsersModel
    {
        public long Count { get; set; }
        public string NextLink { get; set; }
        public IEnumerable<UserModel> Value { get; set; }
    }
}
