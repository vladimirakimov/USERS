using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models
{
    public class CreateRequest
    {
        private readonly CreateFromQuery _query;
        private readonly CreateFromBody _body;

        public CreateRequest(CreateFromQuery query, CreateFromBody body)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
            _body = body ?? throw new ArgumentNullException(nameof(body));
        }

        public string QueryApiVersion => _query.ApiVersion;

        public string BodyLogin => _body.Login;

        public string BodyPassword => _body.Password;

        public string BodyFirstName => _body.FirstName;

        public string BodyLastName => _body.LastName;
    }
}
