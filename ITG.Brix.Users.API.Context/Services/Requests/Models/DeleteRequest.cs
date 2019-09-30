using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models
{
    public class DeleteRequest
    {
        private readonly DeleteFromRoute _route;
        private readonly DeleteFromQuery _query;
        private readonly DeleteFromHeader _header;

        public DeleteRequest(DeleteFromRoute route, DeleteFromQuery query, DeleteFromHeader header)
        {
            _route = route ?? throw new ArgumentNullException(nameof(route));
            _query = query ?? throw new ArgumentNullException(nameof(query));
            _header = header ?? throw new ArgumentNullException(nameof(header));
        }

        public string RouteId => _route.Id;

        public string QueryApiVersion => _query.ApiVersion;

        public string HeaderIfMatch => _header.IfMatch;
    }
}
