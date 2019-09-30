using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Services.Requests.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITG.Brix.Users.API.Context.Services.Requests.Impl
{
    public class ApiRequest : IApiRequest
    {
        private readonly IEnumerable<IRequestValidator> _requestValidators;

        public ApiRequest(IEnumerable<IRequestValidator> requestValidators)
        {
            _requestValidators = requestValidators ?? throw new ArgumentNullException(nameof(requestValidators));
        }

        public ValidationResult Validate<T>(T request)
        {
            return _requestValidators.First(x => x.Type == request.GetType()).Validate(request);
        }
    }
}
