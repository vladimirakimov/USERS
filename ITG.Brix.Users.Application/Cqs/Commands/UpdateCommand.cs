using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.DataTypes;
using MediatR;
using System;

namespace ITG.Brix.Users.Application.Cqs.Commands
{
    public class UpdateCommand : IRequest<Result>
    {
        public Guid Id { get; private set; }

        public Optional<string> Login { get; private set; }

        public Optional<string> Password { get; private set; }

        public Optional<string> FirstName { get; private set; }

        public Optional<string> LastName { get; private set; }

        public int Version { get; private set; }

        public UpdateCommand(Guid id, Optional<string> login, Optional<string> password, Optional<string> firstName, Optional<string> lastName, int version)
        {
            Id = id;
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Version = version;
        }
    }
}
