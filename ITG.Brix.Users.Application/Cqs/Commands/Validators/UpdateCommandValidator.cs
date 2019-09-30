using FluentValidation;
using FluentValidation.Results;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Constants;
using ITG.Brix.Users.Application.Enums;
using ITG.Brix.Users.Application.Extensions;
using ITG.Brix.Users.Application.Resources;

namespace ITG.Brix.Users.Application.Cqs.Commands.Validators
{
    public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
    {
        public UpdateCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().CustomFault(CustomFaultCode.NotFound, CustomFailures.UserNotFound);

            RuleFor(x => x.Login).Custom((elem, context) =>
            {
                if (elem.HasValue && string.IsNullOrWhiteSpace(elem.Value))
                {
                    context.AddFailure(new ValidationFailure("Login", ValidationFailures.LoginCannotBeEmpty) { ErrorCode = ErrorType.ValidationError.ToString() });
                }
            });
            RuleFor(x => x.Login).Custom((elem, context) =>
            {
                if (elem.HasValue && !string.IsNullOrWhiteSpace(elem.Value) && (elem.Value.Length < Consts.CqsValidation.LoginLengthMin || elem.Value.Length > Consts.CqsValidation.LoginLengthMax))
                {
                    context.AddFailure(new ValidationFailure("Login", ValidationFailures.LoginLength) { ErrorCode = ErrorType.ValidationError.ToString() });
                }
            });
            RuleFor(x => x.Login).Must(x => x.Value.StartsWithLetter()).When(x => x.Login.HasValue).ValidationFault(ValidationFailures.LoginFirstLetter);

            RuleFor(x => x.Password).Custom((elem, context) =>
            {
                if (elem.HasValue && string.IsNullOrWhiteSpace(elem.Value))
                {
                    context.AddFailure(new ValidationFailure("Password", ValidationFailures.PasswordCannotBeEmpty) { ErrorCode = ErrorType.ValidationError.ToString() });
                }
            });
            RuleFor(x => x.Password).Custom((elem, context) =>
            {
                if (elem.HasValue && !string.IsNullOrWhiteSpace(elem.Value) && (elem.Value.Length < Consts.CqsValidation.PasswordLengthMin || elem.Value.Length > Consts.CqsValidation.PasswordLengthMax))
                {
                    context.AddFailure(new ValidationFailure("Password", ValidationFailures.PasswordLength) { ErrorCode = ErrorType.ValidationError.ToString() });
                }
            });
            RuleFor(x => x.Password).Custom((elem, context) =>
            {
                if (elem.HasValue && !elem.Value.AtLeastOneSpecialCharacter())
                {
                    context.AddFailure(new ValidationFailure("Password", ValidationFailures.PasswordSpecial) { ErrorCode = ErrorType.ValidationError.ToString() });
                }
            });

            RuleFor(x => x.FirstName).Custom((elem, context) =>
            {
                if (elem.HasValue && (string.IsNullOrEmpty(elem.Value) || string.IsNullOrWhiteSpace(elem.Value)))
                {
                    context.AddFailure(new ValidationFailure("FirstName", ValidationFailures.FirstNameMandatory) { ErrorCode = ErrorType.ValidationError.ToString() });
                }
            });
            RuleFor(x => x.FirstName).Custom((elem, context) =>
            {
                if (elem.HasValue && !string.IsNullOrWhiteSpace(elem.Value) && (elem.Value.Length < Consts.CqsValidation.FirstNameLengthMin || elem.Value.Length > Consts.CqsValidation.FirstNameLengthMax))
                {
                    context.AddFailure(new ValidationFailure("FirstName", ValidationFailures.FirstNameLength) { ErrorCode = ErrorType.ValidationError.ToString() });
                }
            });
            RuleFor(x => x.FirstName).Custom((elem, context) =>
            {
                if (elem.HasValue && !string.IsNullOrWhiteSpace(elem.Value) && (elem.Value.Length >= Consts.CqsValidation.FirstNameLengthMin || elem.Value.Length <= Consts.CqsValidation.FirstNameLengthMax))
                {
                    var result = elem.Value.HasLettersOrSingleSpaceCharacters();
                    if (!result)
                    {
                        context.AddFailure(new ValidationFailure("FirstName", ValidationFailures.FirstNameOnlyLettersAndSingleSpacesAllowed) { ErrorCode = ErrorType.ValidationError.ToString() });
                    }
                }
            });
            RuleFor(x => x.FirstName).Custom((elem, context) =>
            {
                if (elem.HasValue && !string.IsNullOrWhiteSpace(elem.Value) && (elem.Value.Length >= Consts.CqsValidation.FirstNameLengthMin || elem.Value.Length <= Consts.CqsValidation.FirstNameLengthMax))
                {
                    var result = elem.Value.StartsWithCapitalLetter();
                    if (!result)
                    {
                        context.AddFailure(new ValidationFailure("FirstName", ValidationFailures.FirstNameCapitalizedFirstLetter) { ErrorCode = ErrorType.ValidationError.ToString() });
                    }
                }
            });

            RuleFor(x => x.LastName).Custom((elem, context) =>
            {
                if (elem.HasValue && (string.IsNullOrEmpty(elem.Value) || string.IsNullOrWhiteSpace(elem.Value)))
                {
                    context.AddFailure(new ValidationFailure("LastName", ValidationFailures.LastNameMandatory) { ErrorCode = ErrorType.ValidationError.ToString() });
                }
            });
            RuleFor(x => x.LastName).Custom((elem, context) =>
            {
                if (elem.HasValue && !string.IsNullOrWhiteSpace(elem.Value) && (elem.Value.Length < Consts.CqsValidation.LastNameLengthMin || elem.Value.Length > Consts.CqsValidation.LastNameLengthMax))
                {
                    context.AddFailure(new ValidationFailure("LastName", ValidationFailures.LastNameLength) { ErrorCode = ErrorType.ValidationError.ToString() });
                }
            });
            RuleFor(x => x.LastName).Custom((elem, context) =>
            {
                if (elem.HasValue && !string.IsNullOrWhiteSpace(elem.Value) && (elem.Value.Length >= Consts.CqsValidation.LastNameLengthMin || elem.Value.Length <= Consts.CqsValidation.LastNameLengthMax))
                {
                    var result = elem.Value.HasLettersOrSingleSpaceCharacters();
                    if (!result)
                    {
                        context.AddFailure(new ValidationFailure("LastName", ValidationFailures.LastNameOnlyLettersAndSingleSpacesAllowed) { ErrorCode = ErrorType.ValidationError.ToString() });
                    }
                }
            });
            RuleFor(x => x.LastName).Custom((elem, context) =>
            {
                if (elem.HasValue && !string.IsNullOrWhiteSpace(elem.Value) && (elem.Value.Length >= Consts.CqsValidation.LastNameLengthMin || elem.Value.Length <= Consts.CqsValidation.LastNameLengthMax))
                {
                    var result = elem.Value.StartsWithCapitalLetter();
                    if (!result)
                    {
                        context.AddFailure(new ValidationFailure("LastName", ValidationFailures.LastNameCapitalizedFirstLetter) { ErrorCode = ErrorType.ValidationError.ToString() });
                    }
                }
            });
        }
    }
}
