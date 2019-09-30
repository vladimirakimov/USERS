using ITG.Brix.Users.API.Context.Services.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ITG.Brix.Users.API.Context.Services
{
    public interface IApiResult
    {
        Task<IActionResult> Produce(ListRequest request);
        Task<IActionResult> Produce(GetRequest request);
        Task<IActionResult> Produce(CreateRequest request);
        Task<IActionResult> Produce(UpdateRequest request);
        Task<IActionResult> Produce(DeleteRequest request);
        Task<IActionResult> Produce(LoginRequest request);
    }
}
