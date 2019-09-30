using ITG.Brix.Users.API.Context.Services.Arrangements.Bases;
using ITG.Brix.Users.API.Context.Services.Requests.Mappers;
using ITG.Brix.Users.API.Context.Services.Requests.Models;
using ITG.Brix.Users.API.Context.Services.Responses;
using ITG.Brix.Users.Application.Bases;
using ITG.Brix.Users.Application.Cqs.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITG.Brix.Users.API.Context.Services.Arrangements.Impl
{
    public class OperationArrangement : IOperationArrangement
    {
        private readonly IMediator _mediator;
        private readonly IApiResponse _apiResponse;
        private readonly ICqsMapper _cqsMapper;

        public OperationArrangement(IMediator mediator,
                                    IApiResponse apiResponse,
                                    ICqsMapper cqsMapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _apiResponse = apiResponse ?? throw new ArgumentNullException(nameof(apiResponse));
            _cqsMapper = cqsMapper ?? throw new ArgumentNullException(nameof(cqsMapper));
        }

        public async Task<IActionResult> Process(ListRequest request, IValidatorActionResult validatorActionResult)
        {
            IActionResult actionResult;

            if (validatorActionResult.Result == null)
            {
                request.UpdateFilter(fromToSet: new Dictionary<string, string>{
                        { "login", "Login/Value"},
                        { "firstName", "fullName/firstName/value"},
                        { "lastName", "fullName/lastName/value"},
                        { "version", "dissalowed"},
                    });

                var call = _cqsMapper.Map(request);

                var result = await _mediator.Send(call);

                actionResult = result.IsFailure ? _apiResponse.Fail(result)
                                                : _apiResponse.Ok(((Result<UsersModel>)result).Value);
            }
            else
            {
                actionResult = validatorActionResult.Result;
            }

            return actionResult;
        }

        public async Task<IActionResult> Process(GetRequest request, IValidatorActionResult validatorActionResult)
        {
            IActionResult actionResult;

            if (validatorActionResult.Result == null)
            {
                var call = _cqsMapper.Map(request);

                var result = await _mediator.Send(call);

                actionResult = result.IsFailure ? _apiResponse.Fail(result)
                                                : _apiResponse.Ok(((Result<UserModel>)result).Value, ((Result<UserModel>)result).Version.ToString());
            }
            else
            {
                actionResult = validatorActionResult.Result;
            }

            return actionResult;
        }

        public async Task<IActionResult> Process(CreateRequest request, IValidatorActionResult validatorActionResult)
        {
            IActionResult actionResult;

            if (validatorActionResult.Result == null)
            {
                var call = _cqsMapper.Map(request);

                var result = await _mediator.Send(call);

                actionResult = result.IsFailure ? _apiResponse.Fail(result)
                                                : _apiResponse.Created(string.Format("/api/users/{0}", ((Result<Guid>)result).Value), result.Version.ToString());
            }
            else
            {
                actionResult = validatorActionResult.Result;
            }

            return actionResult;
        }

        public async Task<IActionResult> Process(UpdateRequest request, IValidatorActionResult validatorActionResult)
        {
            IActionResult actionResult;

            if (validatorActionResult.Result == null)
            {
                var call = _cqsMapper.Map(request);

                var result = await _mediator.Send(call);

                actionResult = result.IsFailure ? _apiResponse.Fail(result)
                                                : _apiResponse.Updated(result.Version.ToString());
            }
            else
            {
                actionResult = validatorActionResult.Result;
            }

            return actionResult;
        }

        public async Task<IActionResult> Process(DeleteRequest request, IValidatorActionResult validatorActionResult)
        {
            IActionResult actionResult;

            if (validatorActionResult.Result == null)
            {
                var call = _cqsMapper.Map(request);

                var result = await _mediator.Send(call);

                actionResult = result.IsFailure ? _apiResponse.Fail(result)
                                                : _apiResponse.Deleted();
            }
            else
            {
                actionResult = validatorActionResult.Result;
            }

            return actionResult;
        }

        public async Task<IActionResult> Process(LoginRequest request, IValidatorActionResult validatorActionResult)
        {
            IActionResult actionResult;

            if (validatorActionResult.Result == null)
            {
                var call = _cqsMapper.Map(request);

                var result = await _mediator.Send(call);

                actionResult = result.IsFailure ? _apiResponse.Fail(result)
                                                 : _apiResponse.Ok(((Result<AuthenticatedUserModel>)result).Value);
            }
            else
            {
                actionResult = validatorActionResult.Result;
            }

            return actionResult;
        }
    }
}
