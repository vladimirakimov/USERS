using ITG.Brix.Users.Application.Bases;
using MediatR;

namespace ITG.Brix.Users.Application.Cqs.Queries
{
    public class LoginQuery : IRequest<Result>
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public LoginQuery(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
