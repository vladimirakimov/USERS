using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.Application.Cqs.Commands;
using ITG.Brix.Users.Application.Cqs.Queries;
using ITG.Brix.Users.Application.DataTypes;
using ITG.Brix.Users.Application.Extensions;
using ITG.Brix.Users.Infrastructure.Providers;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Mappers.Impl
{
    public class CqsMapper : ICqsMapper
    {
        private readonly IJsonProvider _jsonProvider;

        public CqsMapper(IJsonProvider jsonProvider)
        {
            _jsonProvider = jsonProvider ?? throw new ArgumentNullException(nameof(jsonProvider));
        }

        public ListQuery Map(ListRequest request)
        {
            var filter = request.Filter;
            var top = request.Top;
            var skip = request.Skip;

            var result = new ListQuery(filter, top, skip);
            return result;
        }

        public GetQuery Map(GetRequest request)
        {
            var id = new Guid(request.RouteId);

            var result = new GetQuery(id);
            return result;
        }

        public DeleteCommand Map(DeleteRequest request)
        {
            var id = new Guid(request.RouteId);

            var version = ToVersion(request.HeaderIfMatch);

            var result = new DeleteCommand(id, version);
            return result;
        }

        public CreateCommand Map(CreateRequest request)
        {
            var result = new CreateCommand(request.BodyLogin, request.BodyPassword, request.BodyFirstName, request.BodyLastName);
            return result;
        }

        public UpdateCommand Map(UpdateRequest request)
        {
            var id = new Guid(request.RouteId);

            var valuePairs = _jsonProvider.ToDictionary(request.BodyPatch);

            Optional<string> login = valuePairs.GetOptional("login");
            Optional<string> password = valuePairs.GetOptional("password");
            Optional<string> firstName = valuePairs.GetOptional("fIrstName");
            Optional<string> lastName = valuePairs.GetOptional("lastName");

            var version = ToVersion(request.HeaderIfMatch);

            var result = new UpdateCommand(id, login, password, firstName, lastName, version);
            return result;
        }

        public LoginQuery Map(LoginRequest request)
        {
            var result = new LoginQuery(request.Login, request.Password);

            return result;
        }

        private int ToVersion(string eTag)
        {
            var eTagValue = eTag.Replace("\"", "");
            var result = int.Parse(eTagValue);

            return result;
        }
    }
}
