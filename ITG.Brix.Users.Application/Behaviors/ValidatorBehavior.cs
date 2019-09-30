using FluentValidation;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Enums;
using ITG.Brix.Users.Application.Extensions;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITG.Brix.Users.Application.Behaviors
{
    public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : Result
    {
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorBehavior(IValidator<TRequest>[] validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var customFailures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null && error.ErrorCode.StartsWith(ErrorType.CustomError.ToString()))
                .ToList();

            if (customFailures.Any())
            {
                var failure = customFailures.First();
                var errors = new List<Failure>();
                errors.Add(new CustomFault
                {
                    Code = failure.ErrorCode.Substring((ErrorType.CustomError.ToString() + "###").Length),
                    Message = failure.ErrorMessage,
                    Target = failure.PropertyName.ToCamelCase()
                });
                var result = Result.Fail(errors);
                return await Task.FromResult(result as TResponse);
            }

            var validationFailures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(error => error != null)
                .ToList();

            if (validationFailures.Any())
            {
                var errors = new List<Failure>();
                foreach (var error in validationFailures)
                {
                    errors.Add(new ValidationFault
                    {
                        Code = "invalid-input",
                        Message = error.ErrorMessage,
                        Target = error.PropertyName.ToCamelCase()
                    });
                }
                var result = Result.Fail(errors);
                return await Task.FromResult(result as TResponse);
            }

            return await next();
        }
    }
}
