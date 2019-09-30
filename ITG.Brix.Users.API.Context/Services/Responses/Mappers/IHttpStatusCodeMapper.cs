using ITG.Brix.Users.API.Context.Bases;
using System.Net;

namespace ITG.Brix.Users.API.Context.Services.Responses.Mappers
{
    public interface IHttpStatusCodeMapper
    {
        HttpStatusCode Map(ValidationResult validationResult);
    }
}
