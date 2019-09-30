using ITG.Brix.Users.Domain.Bases;
using ITG.Brix.Users.Domain.Internal;
using System;

namespace ITG.Brix.Users.Domain
{
    public class User : Entity, IAggregateRoot
    {
        public User(Guid id, Login login, string password, FullName fullName)
        {
            if (id == default(Guid))
            {
                throw Error.Argument(string.Format("{0} can't be default guid.", nameof(id)));
            }
            if (object.Equals(login, null))
            {
                throw Error.ArgumentNull(string.Format("{0} can't be null.", nameof(login)));
            }
            if (object.Equals(fullName, null))
            {
                throw Error.ArgumentNull(string.Format("{0} can't be null.", nameof(fullName)));
            }
            if (string.IsNullOrEmpty(password))
            {
                throw Error.ArgumentNull(string.Format("{0} can't be empty.", nameof(password)));
            }

            Id = id;
            Login = login;
            Password = password;
            FullName = fullName;
        }

        [Signature]
        public Login Login { get; private set; }

        public string Password { get; private set; }

        public FullName FullName { get; private set; }

        public int Version { get; set; }

        public void ChangeLogin(Login login)
        {
            Login = login;
        }

        public void ChangePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw Error.ArgumentNull(string.Format("{0} can't be empty.", nameof(password)));
            }

            Password = password;
        }
    }
}
