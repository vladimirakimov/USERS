using ITG.Brix.Users.API.Context.Services.Arrangements.Bases;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ITG.Brix.Users.API.Context.Services.Arrangements
{
    public interface IOperationArrangement
    {
        Task<IActionResult> Process(ListRequest request, IValidatorActionResult validatorActionResult);
        Task<IActionResult> Process(GetRequest request, IValidatorActionResult validatorActionResult);
        Task<IActionResult> Process(CreateRequest request, IValidatorActionResult validatorActionResult);
        Task<IActionResult> Process(UpdateRequest request, IValidatorActionResult validatorActionResult);
        Task<IActionResult> Process(DeleteRequest request, IValidatorActionResult validatorActionResult);
        Task<IActionResult> Process(LoginRequest request, IValidatorActionResult validatorActionResult);
    }
}
