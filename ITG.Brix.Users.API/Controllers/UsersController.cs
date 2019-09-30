using ITG.Brix.Users.API.Context.Services;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using ITG.Brix.Users.API.Context.Services.Responses.Models.Errors;
using ITG.Brix.Users.API.Extensions;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ITG.Brix.Users.API.Controllers
{
    [ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IApiResult _apiResult;

        public UsersController(IApiResult apiResult)
        {
            _apiResult = apiResult ?? throw new ArgumentNullException(nameof(apiResult));
        }

        [HttpGet]
        [ProducesResponseType(typeof(UsersModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> List([FromQuery] ListFromQuery query)
        {
            var request = new ListRequest(query);

            var result = await _apiResult.Produce(request);

            return result;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromRoute] GetFromRoute route,
                                             [FromQuery] GetFromQuery query)
        {
            var request = new GetRequest(route, query);

            var result = await _apiResult.Produce(request);

            return result;
        }


        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.UnsupportedMediaType)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromQuery] CreateFromQuery query,
                                                [FromBody] CreateFromBody body)
        {
            var request = new CreateRequest(query, body);

            var result = await _apiResult.Produce(request);

            return result;
        }

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> Login([FromQuery] LoginFromQuery query,
                                               [FromBody] LoginFromBody body)
        {
            var request = new LoginRequest(query, body);

            var result = await _apiResult.Produce(request);

            return result;
        }


        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.PreconditionFailed)]
        public async Task<IActionResult> Delete([FromRoute] DeleteFromRoute route,
                                                [FromQuery] DeleteFromQuery query,
                                                [FromHeader] DeleteFromHeader header)
        {
            var request = new DeleteRequest(route, query, header);

            var result = await _apiResult.Produce(request);

            return result;
        }

        [HttpPatch("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.UnsupportedMediaType)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseError), (int)HttpStatusCode.PreconditionFailed)]
        public async Task<IActionResult> Update([FromRoute] UpdateFromRoute route,
                                                [FromQuery] UpdateFromQuery query,
                                                [FromHeader] UpdateFromHeader header)
        {

            string bodyAsString = await Request.GetRawBodyStringAsync();
            var body = new UpdateFromBody { Patch = bodyAsString };

            var request = new UpdateRequest(route, query, header, body);

            var result = await _apiResult.Produce(request);

            return result;
        }

    }
}