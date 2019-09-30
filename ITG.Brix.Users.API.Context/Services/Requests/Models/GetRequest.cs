using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models
{
    public class GetRequest
    {
        private readonly GetFromRoute _route;
        private readonly GetFromQuery _query;

        public GetRequest(GetFromRoute route, GetFromQuery query)
        {
            _route = route ?? throw new ArgumentNullException(nameof(route));
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }

        public string RouteId => _route.Id;

        public string QueryApiVersion => _query.ApiVersion;
    }
}
