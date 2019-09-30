using ITG.Brix.Users.API.Context.Constants;
using ITG.Brix.Users.API.Context.Resources;

namespace ITG.Brix.Users.API.Context.Bases
{
    /// <summary>
    /// Safe service-defined error codes.
    /// </summary>
    public class ServiceError
    {
        public static readonly ServiceError None = new ServiceError(Consts.Failure.ServiceErrorCode.None, i18n(Consts.Failure.ServiceErrorCode.None));

        public static readonly ServiceError AuthenticationFailed = new ServiceError(Consts.Failure.ServiceErrorCode.AuthenticationFailed, i18n(Consts.Failure.ServiceErrorCode.AuthenticationFailed));
        public static readonly ServiceError UnsupportedMediaType = new ServiceError(Consts.Failure.ServiceErrorCode.UnsupportedMediaType, i18n(Consts.Failure.ServiceErrorCode.UnsupportedMediaType));
        public static readonly ServiceError InvalidInput = new ServiceError(Consts.Failure.ServiceErrorCode.InvalidInput, i18n(Consts.Failure.ServiceErrorCode.InvalidInput));
        public static readonly ServiceError InvalidQueryParameterValue = new ServiceError(Consts.Failure.ServiceErrorCode.InvalidQueryParameterValue, i18n(Consts.Failure.ServiceErrorCode.InvalidQueryParameterValue));
        public static readonly ServiceError InvalidHeaderValue = new ServiceError(Consts.Failure.ServiceErrorCode.InvalidHeaderValue, i18n(Consts.Failure.ServiceErrorCode.InvalidHeaderValue));
        public static readonly ServiceError InvalidRequestBodyValue = new ServiceError(Consts.Failure.ServiceErrorCode.InvalidRequestBodyValue, i18n(Consts.Failure.ServiceErrorCode.InvalidRequestBodyValue));
        public static readonly ServiceError ResourceNotFound = new ServiceError(Consts.Failure.ServiceErrorCode.ResourceNotFound, i18n(Consts.Failure.ServiceErrorCode.ResourceNotFound));
        public static readonly ServiceError ResourceAlreadyExists = new ServiceError(Consts.Failure.ServiceErrorCode.ResourceAlreadyExists, i18n(Consts.Failure.ServiceErrorCode.ResourceAlreadyExists));
        public static readonly ServiceError ConditionNotMet = new ServiceError(Consts.Failure.ServiceErrorCode.ConditionNotMet, i18n(Consts.Failure.ServiceErrorCode.ConditionNotMet));
        public static readonly ServiceError MissingRequiredQueryParameter = new ServiceError(Consts.Failure.ServiceErrorCode.MissingRequiredQueryParameter, i18n(Consts.Failure.ServiceErrorCode.MissingRequiredQueryParameter));
        public static readonly ServiceError MissingRequiredHeader = new ServiceError(Consts.Failure.ServiceErrorCode.MissingRequiredHeader, i18n(Consts.Failure.ServiceErrorCode.MissingRequiredHeader));
        public static readonly ServiceError MissingRequiredRequestBody = new ServiceError(Consts.Failure.ServiceErrorCode.MissingRequiredRequestBody, i18n(Consts.Failure.ServiceErrorCode.MissingRequiredRequestBody));


        /// <summary>
        /// Service-defined error code. This code serves as a sub-status for the HTTP error code.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Human-readable representation of the error.
        /// </summary>
        public string Message { get; private set; }

        private ServiceError() { }

        public ServiceError(string code, string message)
        {
            Code = code;
            Message = message;
        }

        private static string i18n(string key)
        {
            var result = ServiceErrorMessages.ResourceManager.GetString(key);
            return result;
        }

    }
}
