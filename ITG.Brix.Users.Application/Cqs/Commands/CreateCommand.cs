using ITG.Brix.Users.Application.Bases;
using MediatR;

namespace ITG.Brix.Users.Application.Cqs.Commands
{
    public class CreateCommand : IRequest<Result>
    {
        public string Login { get; private set; }

        public string Password { get; private set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public CreateCommand(string login, string password, string firstName, string lastName)
        {
            Login = login;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
