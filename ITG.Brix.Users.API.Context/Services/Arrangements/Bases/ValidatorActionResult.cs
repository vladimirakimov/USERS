using Microsoft.AspNetCore.Mvc;

namespace ITG.Brix.Users.API.Context.Services.Arrangements.Bases
{
    public class ValidatorActionResult : IValidatorActionResult
    {
        private readonly IActionResult _result;

        public ValidatorActionResult(IActionResult result)
        {
            _result = result;
        }
        public IActionResult Result => _result;
    }
}
