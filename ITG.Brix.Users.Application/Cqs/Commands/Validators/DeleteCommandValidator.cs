using FluentValidation;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Extensions;
using ITG.Brix.Users.Application.Resources;

namespace ITG.Brix.Users.Application.Cqs.Commands.Validators
{
    public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
    {
        public DeleteCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().CustomFault(CustomFaultCode.NotFound, CustomFailures.UserNotFound);
        }
    }
}
