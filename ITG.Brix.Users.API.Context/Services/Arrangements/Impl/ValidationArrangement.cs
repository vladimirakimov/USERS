using ITG.Brix.Users.API.Context.Services.Arrangements.Bases;
using ITG.Brix.Users.API.Context.Services.Requests;
using ITG.Brix.Users.API.Context.Services.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ITG.Brix.Users.API.Context.Services.Arrangements.Impl
{
    public class ValidationArrangement : IValidationArrangement
    {
        private readonly IApiRequest _apiRequest;
        private readonly IApiResponse _apiResponse;

        public ValidationArrangement(IApiRequest apiRequest,
                                     IApiResponse apiResponse)
        {
            _apiRequest = apiRequest ?? throw new ArgumentNullException(nameof(apiRequest));
            _apiResponse = apiResponse ?? throw new ArgumentNullException(nameof(apiResponse));
        }

        public async Task<IValidatorActionResult> Validate<T>(T request)
        {
            IActionResult actionResult = null;

            var validationResult = _apiRequest.Validate(request);

            if (validationResult.HasErrors)
            {
                actionResult = _apiResponse.Fail(validationResult);
            }

            var result = new ValidatorActionResult(actionResult);
            return result;
        }
    }
}
