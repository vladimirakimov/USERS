using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Services.Responses.Models.Errors;

namespace ITG.Brix.Users.API.Context.Services.Responses.Mappers
{
    public interface IErrorMapper
    {
        ResponseError Map(ValidationResult validationResult);
    }
}
