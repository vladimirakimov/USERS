using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.Application.Bases;
using Microsoft.AspNetCore.Mvc;

namespace ITG.Brix.Users.API.Context.Services.Responses
{
    public interface IApiResponse
    {
        IActionResult Fail(ValidationResult validationResult);
        IActionResult Fail(Result result);
        IActionResult Ok(object value);
        IActionResult Ok(object value, string eTagValue);
        IActionResult Created(string uri, string eTagValue);
        IActionResult Updated(string eTagValue);
        IActionResult Deleted();
        IActionResult Error();
    }
}
