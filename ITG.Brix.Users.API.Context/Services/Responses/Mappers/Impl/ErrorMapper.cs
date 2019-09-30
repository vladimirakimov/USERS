using ITG.Brix.Users.API.Context.Bases;
using ITG.Brix.Users.API.Context.Services.Responses.Models.Errors;
using System.Collections.Generic;
using System.Linq;

namespace ITG.Brix.Users.API.Context.Services.Responses.Mappers.Impl
{
    public class ErrorMapper : IErrorMapper
    {
        public ResponseError Map(ValidationResult validationResult)
        {
            if (!validationResult.HasErrors)
            {
                return null;
            }

            var responseError = new ResponseError { Error = new ResponseErrorBody { Code = validationResult.ServiceError.Code, Message = validationResult.ServiceError.Message } };
            if (validationResult.Errors.Any())
            {
                responseError.Error.Details = new List<ResponseErrorField>();
                foreach (var validationError in validationResult.Errors)
                {
                    var responseErrorField = new ResponseErrorField() { Code = validationError.Code, Message = validationError.Message, Target = validationError.Target };
                    responseError.Error.Details.Add(responseErrorField);
                }
            }

            return responseError;
        }
    }
}
