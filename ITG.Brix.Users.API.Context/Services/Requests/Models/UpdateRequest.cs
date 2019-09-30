using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models
{
    public class UpdateRequest
    {
        private readonly UpdateFromRoute _route;
        private readonly UpdateFromQuery _query;
        private readonly UpdateFromHeader _header;
        private readonly UpdateFromBody _body;

        public UpdateRequest(UpdateFromRoute route, UpdateFromQuery query, UpdateFromHeader header, UpdateFromBody body)
        {
            _route = route ?? throw new ArgumentNullException(nameof(route));
            _query = query ?? throw new ArgumentNullException(nameof(query));
            _header = header ?? throw new ArgumentNullException(nameof(header));
            _body = body ?? throw new ArgumentNullException(nameof(body));
        }

        public string RouteId => _route.Id;

        public string QueryApiVersion => _query.ApiVersion;

        public string HeaderIfMatch => _header.IfMatch;

        public string HeaderContentType => _header.ContentType;

        public string BodyPatch => _body.Patch;
    }
}
