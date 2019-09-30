using FluentValidation;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Extensions;
using ITG.Brix.Users.Application.Resources;

namespace ITG.Brix.Users.Application.Cqs.Queries.Validators
{
    public class GetQueryValidator : AbstractValidator<GetQuery>
    {
        public GetQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().CustomFault(CustomFaultCode.NotFound, CustomFailures.UserNotFound);
        }
    }
}
