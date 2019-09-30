using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Requests.Validators.Components;
using ITG.Brix.Users.API.Context.Services.Requests.Validators.Impl.Bases;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Validators.Impl
{
    public class LoginRequestValidator : AbstractRequestValidator<LoginRequest>
    {
        private readonly IRequestComponentValidator _requestComponentValidator;

        public LoginRequestValidator(IRequestComponentValidator requestComponentValidator)
        {
            _requestComponentValidator = requestComponentValidator ?? throw new ArgumentNullException(nameof(requestComponentValidator));
        }

        public override ValidationResult Validate<T>(T request)
        {
            var req = request as LoginRequest;

            ValidationResult result;

            result = _requestComponentValidator.QueryApiVersionRequired(req.ApiVersion);

            if (result == null)
            {
                result = _requestComponentValidator.QueryApiVersion(req.ApiVersion);
            }

            if (result == null)
            {
                result = new ValidationResult();
            }

            return result;
        }
    }
}
