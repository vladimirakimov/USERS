using FluentValidation.Results;
using FluentValidation.Validators;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Enums;

namespace ITG.Brix.Users.Application.Extensions
{
    public static class CustomContextExtensions
    {
        public static void AddValidationFault(this CustomContext context, string property, string errorMessage)
        {
            context.AddFailure(new ValidationFailure(property, errorMessage) { ErrorCode = ErrorType.ValidationError.ToString() });
        }

        public static void AddCustomFault(this CustomContext context, string property, CustomFaultCode errorCode, string errorMessage)
        {
            context.AddFailure(new ValidationFailure(property, errorMessage) { ErrorCode = ErrorType.CustomError.ToString() + "###" + errorCode.Name });
        }
    }
}
