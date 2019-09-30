using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Services.Responses.Mappers;
using ITG.Brix.Users.API.Context.Services.Responses.Results;
using ITG.Brix.Users.Application.Bases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace ITG.Brix.Users.API.Context.Services.Responses.Impl
{
    public class ApiResponse : IApiResponse
    {
        private readonly IErrorMapper _errorMapper;
        private readonly IHttpStatusCodeMapper _httpStatusCodeMapper;

        private readonly Controller _controller;

        public ApiResponse(IErrorMapper errorMapper,
                           IHttpStatusCodeMapper httpStatusCodeMapper)
        {
            _errorMapper = errorMapper ?? throw new ArgumentNullException(nameof(errorMapper));
            _httpStatusCodeMapper = httpStatusCodeMapper ?? throw new ArgumentNullException(nameof(httpStatusCodeMapper));

            _controller = new Controller();
        }

        public IActionResult Fail(ValidationResult validationResult)
        {
            var error = _errorMapper.Map(validationResult);
            var httpStatusCode = _httpStatusCodeMapper.Map(validationResult);
            switch (httpStatusCode)
            {
                case HttpStatusCode.UnsupportedMediaType:
                    return _controller.StatusCode((int)httpStatusCode, error);
                case HttpStatusCode.BadRequest:
                    return _controller.BadRequest(error);
                case HttpStatusCode.NotFound:
                    return _controller.NotFound(error);
                case HttpStatusCode.Conflict:
                    return _controller.Conflict(error);
                case HttpStatusCode.PreconditionFailed:
                    return _controller.StatusCode((int)httpStatusCode, error);
                default:
                    return _controller.StatusCode((int)httpStatusCode);
            }
        }

        public IActionResult Fail(Result result)
        {
            var validationResult = new ValidationResult();

            foreach (var error in result.Failures)
            {
                if (error is CustomFault)
                {
                    switch (error.Code)
                    {
                        case "not-found":
                            validationResult.ServiceError = ServiceError.ResourceNotFound;
                            validationResult.Errors = GetFailureList(error);
                            break;
                        case "invalid-query-top":
                        case "invalid-query-skip":
                            validationResult.ServiceError = ServiceError.InvalidQueryParameterValue;
                            validationResult.Errors = GetFailureList(error);
                            break;
                        default:
                            throw new NotImplementedException(error.Code);
                    }
                    break;
                }
                else if (error is ValidationFault)
                {

                    validationResult.ServiceError = ServiceError.InvalidInput;
                    validationResult.Errors = GetFailureList(error);

                    break;
                }
                else
                {
                    validationResult.Errors = GetFailureList(error);
                    switch (error.Code)
                    {
                        case "not-found":
                            validationResult.ServiceError = ServiceError.ResourceNotFound;
                            validationResult.Errors = GetFailureList(error);
                            break;
                        case "not-met":
                            validationResult.ServiceError = ServiceError.ConditionNotMet;
                            validationResult.Errors = GetFailureList(error, true);
                            break;
                        case "conflict":
                            validationResult.ServiceError = ServiceError.ResourceAlreadyExists;
                            validationResult.Errors = GetFailureList(error);
                            break;
                        case "invalid":
                            validationResult.ServiceError = ServiceError.InvalidInput;
                            validationResult.Errors = GetFailureList(error);
                            break;
                        case "invalid-query-filter":
                            validationResult.ServiceError = ServiceError.InvalidQueryParameterValue;
                            validationResult.Errors = GetFailureList(error);
                            break;
                        case "invalid-credentials":
                            validationResult.ServiceError = ServiceError.AuthenticationFailed;
                            validationResult.Errors = GetFailureList(error);
                            break;
                        default:
                            throw new NotImplementedException(error.Code);
                    }
                    break;
                }
            }
            return Fail(validationResult);
        }

        public IActionResult Ok(object value)
        {
            return _controller.Ok(value);
        }

        public IActionResult Ok(object value, string eTagValue)
        {
            return new CustomOkResult(value, eTagValue);
        }

        public IActionResult Created(string uri, string eTagValue)
        {
            return new CustomCreatedResult(uri, eTagValue);
        }

        public IActionResult Updated(string eTagValue)
        {
            return new CustomUpdatedResult(eTagValue);
        }

        public IActionResult Deleted()
        {
            return _controller.NoContent();
        }

        public IActionResult Error()
        {
            return _controller.StatusCode(StatusCodes.Status500InternalServerError);
        }

        class Controller : ControllerBase
        {
        }

        private List<Failure> GetFailureList(Failure error, bool reverseMapTarget = false)
        {
            var result = new List<Failure>() {
                new Failure {
                     Code = error.Code,
                     Message = error.Message,
                     Target = reverseMapTarget ? ReverseMapTarget(error.Target) : error.Target
                }
            };
            return result;
        }

        private string ReverseMapTarget(string target)
        {
            switch (target)
            {
                case "version":
                    return "if-match";
                default:
                    return string.Empty;
            }
        }
    }
}
