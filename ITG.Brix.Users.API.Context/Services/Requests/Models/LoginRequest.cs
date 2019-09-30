using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models
{
    public class LoginRequest
    {
        private readonly LoginFromBody _body;
        private readonly LoginFromQuery _query;

        public LoginRequest(LoginFromQuery query,
                            LoginFromBody body)
        {
            _body = body ?? throw new ArgumentNullException(nameof(body));
            _query = query ?? throw new ArgumentException(nameof(query));
        }

        public string ApiVersion => _query.ApiVersion;
        public string Login => _body.Login;
        public string Password => _body.Password;
    }
}
