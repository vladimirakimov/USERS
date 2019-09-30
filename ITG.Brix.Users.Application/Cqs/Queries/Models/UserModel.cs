using System;

namespace ITG.Brix.Users.Application.Cqs.Queries.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
