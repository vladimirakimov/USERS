using ITG.Brix.Users.API.Context.Services.Arrangements;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ITG.Brix.Users.API.Context.Services.Impl
{
    public class ApiResult : IApiResult
    {
        private readonly IValidationArrangement _validationArrangement;
        private readonly IOperationArrangement _operationArrangement;

        public ApiResult(IValidationArrangement validationArrangement,
                         IOperationArrangement operationArrangement)
        {
            _validationArrangement = validationArrangement ?? throw new ArgumentNullException(nameof(validationArrangement));
            _operationArrangement = operationArrangement ?? throw new ArgumentNullException(nameof(operationArrangement));
        }

        public async Task<IActionResult> Produce(ListRequest request)
        {
            var validatorActionResult = await _validationArrangement.Validate(request);
            var actionResult = await _operationArrangement.Process(request, validatorActionResult);

            return actionResult;
        }

        public async Task<IActionResult> Produce(GetRequest request)
        {
            var validatorActionResult = await _validationArrangement.Validate(request);
            var actionResult = await _operationArrangement.Process(request, validatorActionResult);

            return actionResult;
        }

        public async Task<IActionResult> Produce(CreateRequest request)
        {
            var validatorActionResult = await _validationArrangement.Validate(request);
            var actionResult = await _operationArrangement.Process(request, validatorActionResult);

            return actionResult;
        }

        public async Task<IActionResult> Produce(UpdateRequest request)
        {
            var validatorActionResult = await _validationArrangement.Validate(request);
            var actionResult = await _operationArrangement.Process(request, validatorActionResult);

            return actionResult;
        }

        public async Task<IActionResult> Produce(DeleteRequest request)
        {
            var validatorActionResult = await _validationArrangement.Validate(request);
            var actionResult = await _operationArrangement.Process(request, validatorActionResult);

            return actionResult;
        }

        public async Task<IActionResult> Produce(LoginRequest request)
        {
            var validatorActionResult = await _validationArrangement.Validate(request);
            var actionResult = await _operationArrangement.Process(request, validatorActionResult);

            return actionResult;
        }
    }
}
