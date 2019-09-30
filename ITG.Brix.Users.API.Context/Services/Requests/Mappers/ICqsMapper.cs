using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.Application.Cqs.Commands;
using ITG.Brix.Users.Application.Cqs.Queries;

namespace ITG.Brix.Users.API.Context.Services.Requests.Mappers
{
    public interface ICqsMapper
    {
        ListQuery Map(ListRequest request);
        GetQuery Map(GetRequest request);
        DeleteCommand Map(DeleteRequest request);
        CreateCommand Map(CreateRequest request);
        UpdateCommand Map(UpdateRequest request);
        LoginQuery Map(LoginRequest request);
    }
}
