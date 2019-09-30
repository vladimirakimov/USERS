using FluentValidation;
using FluentValidation.Results;
using ITG.Brix.Users.Application.Constants;
using ITG.Brix.Users.Application.Enums;
using ITG.Brix.Users.Application.Extensions;
using ITG.Brix.Users.Application.Resources;

namespace ITG.Brix.Users.Application.Cqs.Commands.Validators
{
    public class CreateCommandValidator : AbstractValidator<CreateCommand>
    {
        public CreateCommandValidator()
        {
            RuleFor(x => x.Login).NotEmpty().ValidationFault(ValidationFailures.LoginMandatory);
            RuleFor(x => x.Login).Length(Consts.CqsValidation.LoginLengthMin, Consts.CqsValidation.LoginLengthMax).ValidationFault(ValidationFailures.LoginLength);
            RuleFor(x => x.Login).Must(x => x.StartsWithLetter()).ValidationFault(ValidationFailures.LoginFirstLetter);

            RuleFor(x => x.Password).NotEmpty().ValidationFault(ValidationFailures.PasswordMandatory);
            RuleFor(x => x.Password).Length(Consts.CqsValidation.PasswordLengthMin, Consts.CqsValidation.PasswordLengthMax).ValidationFault(ValidationFailures.PasswordLength);
            RuleFor(x => x.Password).Must(x => x.AtLeastOneSpecialCharacter()).ValidationFault(ValidationFailures.PasswordSpecial);

            RuleFor(x => x.FirstName).NotEmpty().ValidationFault(ValidationFailures.FirstNameMandatory);
            RuleFor(x => x.FirstName).Length(3, 20).When(x => !string.IsNullOrWhiteSpace(x.FirstName)).ValidationFault(ValidationFailures.FirstNameLength);
            RuleFor(x => x.FirstName).Custom((elem, context) =>
            {
                if (!string.IsNullOrWhiteSpace(elem) && (elem.Length >= Consts.CqsValidation.FirstNameLengthMin || elem.Length <= Consts.CqsValidation.FirstNameLengthMax))
                {
                    var result = elem.HasLettersOrSingleSpaceCharacters();
                    if (!result)
                    {
                        context.AddFailure(new ValidationFailure("FirstName", ValidationFailures.FirstNameOnlyLettersAndSingleSpacesAllowed) { ErrorCode = ErrorType.ValidationError.ToString() });
                    }
                }
            });
            RuleFor(x => x.FirstName).Custom((elem, context) =>
            {
                if (!string.IsNullOrWhiteSpace(elem) && (elem.Length >= Consts.CqsValidation.FirstNameLengthMin || elem.Length <= Consts.CqsValidation.FirstNameLengthMax))
                {
                    var result = elem.StartsWithCapitalLetter();
                    if (!result)
                    {
                        context.AddFailure(new ValidationFailure("FirstName", ValidationFailures.FirstNameCapitalizedFirstLetter) { ErrorCode = ErrorType.ValidationError.ToString() });
                    }
                }
            });

            RuleFor(x => x.LastName).NotEmpty().ValidationFault(ValidationFailures.LastNameMandatory);
            RuleFor(x => x.LastName).Length(Consts.CqsValidation.LastNameLengthMin, Consts.CqsValidation.LastNameLengthMax).When(x => !string.IsNullOrWhiteSpace(x.LastName)).ValidationFault(ValidationFailures.LastNameLength);
            RuleFor(x => x.LastName).Custom((elem, context) =>
            {
                if (!string.IsNullOrWhiteSpace(elem) && (elem.Length >= Consts.CqsValidation.LastNameLengthMin || elem.Length <= Consts.CqsValidation.LastNameLengthMax))
                {
                    var result = elem.HasLettersOrSingleSpaceCharacters();
                    if (!result)
                    {
                        context.AddFailure(new ValidationFailure("LastName", ValidationFailures.LastNameOnlyLettersAndSingleSpacesAllowed) { ErrorCode = ErrorType.ValidationError.ToString() });
                    }
                }
            });
            RuleFor(x => x.LastName).Custom((elem, context) =>
            {
                if (!string.IsNullOrWhiteSpace(elem) && (elem.Length >= Consts.CqsValidation.LastNameLengthMin || elem.Length <= Consts.CqsValidation.LastNameLengthMax))
                {
                    var result = elem.StartsWithCapitalLetter();
                    if (!result)
                    {
                        context.AddFailure(new ValidationFailure("LastName", ValidationFailures.LastNameCapitalizedFirstLetter) { ErrorCode = ErrorType.ValidationError.ToString() });
                    }
                }
            });
        }
    }
}
