using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Requests.Validators.Components;
using ITG.Brix.Users.API.Context.Services.Requests.Validators.Impl.Bases;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Validators.Impl
{
    public class CreateRequestValidator : AbstractRequestValidator<CreateRequest>
    {
        private readonly IRequestComponentValidator _requestComponentValidator;

        public CreateRequestValidator(IRequestComponentValidator requestComponentValidator)
        {
            _requestComponentValidator = requestComponentValidator ?? throw new ArgumentNullException(nameof(requestComponentValidator));
        }

        public override ValidationResult Validate<T>(T request)
        {
            var req = request as CreateRequest;

            ValidationResult result;

            result = _requestComponentValidator.QueryApiVersionRequired(req.QueryApiVersion);

            if (result == null)
            {
                result = _requestComponentValidator.QueryApiVersion(req.QueryApiVersion);
            }

            if (result == null)
            {
                result = new ValidationResult();
            }

            return result;
        }
    }
}
