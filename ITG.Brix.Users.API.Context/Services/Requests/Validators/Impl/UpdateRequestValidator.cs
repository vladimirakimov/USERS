using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Requests.Validators.Components;
using ITG.Brix.Users.API.Context.Services.Requests.Validators.Impl.Bases;
using System;

namespace ITG.Brix.Users.API.Context.Services.Requests.Validators.Impl
{
    public class UpdateRequestValidator : AbstractRequestValidator<UpdateRequest>
    {
        private readonly IRequestComponentValidator _requestComponentValidator;

        public UpdateRequestValidator(IRequestComponentValidator requestComponentValidator)
        {
            _requestComponentValidator = requestComponentValidator ?? throw new ArgumentNullException(nameof(requestComponentValidator));
        }

        public override ValidationResult Validate<T>(T request)
        {
            var req = request as UpdateRequest;

            ValidationResult result;

            result = _requestComponentValidator.RouteId(req.RouteId);

            if (result == null)
            {
                result = _requestComponentValidator.QueryApiVersionRequired(req.QueryApiVersion);
            }

            if (result == null)
            {
                result = _requestComponentValidator.QueryApiVersion(req.QueryApiVersion);
            }

            if (result == null)
            {
                result = _requestComponentValidator.HeaderIfMatchRequired(req.HeaderIfMatch);
            }

            if (result == null)
            {
                result = _requestComponentValidator.HeaderIfMatch(req.HeaderIfMatch);
            }

            if (result == null)
            {
                result = _requestComponentValidator.HeaderContentTypeRequired(req.HeaderContentType);
            }

            if (result == null)
            {
                result = _requestComponentValidator.HeaderContentType(req.HeaderContentType);
            }

            if (result == null)
            {
                result = _requestComponentValidator.BodyPatchRequired(req.BodyPatch);
            }

            if (result == null)
            {
                result = _requestComponentValidator.BodyPatch(req.BodyPatch);
            }

            if (result == null)
            {
                result = new ValidationResult();
            }

            return result;
        }
    }
}
