using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Constants;
using ITG.Brix.Users.API.Context.Resources;
using ITG.Brix.Users.Application.Bases;
using System;
using System.Collections.Generic;

namespace ITG.Brix.Users.API.Context.Services.Requests.Validators.Components.Impl
{
    public class RequestComponentValidator : IRequestComponentValidator
    {
        public ValidationResult RouteId(string id)
        {
            ValidationResult result = null;
            if (!Guid.TryParse(id, out Guid identifier))
            {
                result = new ValidationResult
                {
                    ServiceError = ServiceError.ResourceNotFound,
                    Errors = new List<Failure>() {
                        new Failure {
                            Code = Consts.Failure.Detail.Code.NotFound,
                            Message = string.Format(RequestFailures.EntityNotFoundByIdentifier,"User"),
                            Target = Consts.Failure.Detail.Target.Id
                        }
                    }
                };

            }
            return result;
        }

        public ValidationResult QueryApiVersionRequired(string apiVersion)
        {
            ValidationResult result = null;

            if (apiVersion == null)
            {
                result = new ValidationResult
                {
                    ServiceError = ServiceError.MissingRequiredQueryParameter,
                    Errors = new List<Failure>() {
                        new Failure {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.QueryParameterRequired, "api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                };
            }
            return result;
        }

        public ValidationResult QueryApiVersion(string apiVersion)
        {
            ValidationResult result = null;

            if (apiVersion != "1.0")
            {
                result = new ValidationResult
                {
                    ServiceError = ServiceError.InvalidQueryParameterValue,
                    Errors = new List<Failure>() {
                        new Failure {
                            Code = Consts.Failure.Detail.Code.Invalid,
                            Message = string.Format(RequestFailures.QueryParameterInvalidValue,"api-version"),
                            Target = Consts.Failure.Detail.Target.ApiVersion
                        }
                    }
                };
            }
            return result;
        }

        public ValidationResult HeaderIfMatchRequired(string ifMatch)
        {
            ValidationResult result = null;

            if (ifMatch == null)
            {
                result = new ValidationResult
                {
                    ServiceError = ServiceError.MissingRequiredHeader,
                    Errors = new List<Failure>() {
                        new Failure {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.HeaderRequired, "If-Match"),
                            Target = Consts.Failure.Detail.Target.IfMatch
                        }
                    }
                };
            }
            return result;
        }

        public ValidationResult HeaderIfMatch(string ifMatch)
        {
            ValidationResult result = null;

            if (ifMatch != null)
            {
                ifMatch = ifMatch.Replace("\"", "");
            }
            if (string.IsNullOrWhiteSpace(ifMatch))
            {
                result = new ValidationResult
                {
                    ServiceError = ServiceError.InvalidHeaderValue,
                    Errors = new List<Failure>() {
                        new Failure {
                            Code = Consts.Failure.Detail.Code.Invalid,
                            Message = string.Format(RequestFailures.HeaderInvalidValue, "If-Match"),
                            Target = Consts.Failure.Detail.Target.IfMatch
                        }
                    }
                };
            }
            return result;
        }

        public ValidationResult HeaderContentTypeRequired(string contentType)
        {
            ValidationResult result = null;

            if (contentType == null)
            {
                result = new ValidationResult
                {
                    ServiceError = ServiceError.MissingRequiredHeader,
                    Errors = new List<Failure>() {
                        new Failure {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = string.Format(RequestFailures.HeaderRequired, "Content-Type"),
                            Target = Consts.Failure.Detail.Target.ContentType
                        }
                    }
                };
            }
            return result;
        }

        public ValidationResult HeaderContentType(string contentType)
        {
            ValidationResult result = null;

            if (!contentType.Contains("json"))
            {
                result = new ValidationResult
                {
                    ServiceError = ServiceError.UnsupportedMediaType,
                    Errors = new List<Failure>() {
                        new Failure {
                            Code = Consts.Failure.Detail.Code.Unsupported,
                            Message = string.Format(RequestFailures.HeaderUnsupportedValue, "Content-Type"),
                            Target = Consts.Failure.Detail.Target.ContentType
                        }
                    }
                };
            }
            return result;
        }

        public ValidationResult BodyPatchRequired(string patch)
        {
            ValidationResult result = null;

            if (patch == null)
            {
                result = new ValidationResult
                {
                    ServiceError = ServiceError.MissingRequiredRequestBody,
                    Errors = new List<Failure>() {
                        new Failure {
                            Code = Consts.Failure.Detail.Code.Missing,
                            Message = RequestFailures.RequestBodyRequired,
                            Target = Consts.Failure.Detail.Target.RequestBody
                        }
                    }
                };
            }
            return result;
        }

        public ValidationResult BodyPatch(string patch)
        {
            ValidationResult result = null;

            if (string.IsNullOrWhiteSpace(patch))
            {
                result = new ValidationResult
                {
                    ServiceError = ServiceError.InvalidRequestBodyValue,
                    Errors = new List<Failure>() {
                        new Failure {
                            Code = Consts.Failure.Detail.Code.Invalid,
                            Message = RequestFailures.RequestBodyInvalidValue,
                            Target = Consts.Failure.Detail.Target.RequestBody
                        }
                    }
                };
            }
            return result;
        }
    }
}
