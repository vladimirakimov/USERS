using Microsoft.AspNetCore.Mvc;

namespace ITG.Brix.Users.API.Context.Services.Arrangements.Bases
{
    public interface IValidatorActionResult
    {
        IActionResult Result { get; }
    }
}
