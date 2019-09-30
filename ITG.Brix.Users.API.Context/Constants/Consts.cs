namespace ITG.Brix.Users.API.Context.Constants
{
    internal static class Consts
    {
        internal static class Failure
        {
            internal static class ServiceErrorCode
            {
                internal const string None = "None";
                internal const string UnsupportedMediaType = "UnsupportedMediaType";
                internal const string AuthenticationFailed = "AuthenticationFailed";
                internal const string InvalidInput = "InvalidInput";
                internal const string InvalidQueryParameterValue = "InvalidQueryParameterValue";
                internal const string InvalidHeaderValue = "InvalidHeaderValue";
                internal const string InvalidRequestBodyValue = "InvalidRequestBodyValue";
                internal const string ResourceNotFound = "ResourceNotFound";
                internal const string ResourceAlreadyExists = "ResourceAlreadyExists";
                internal const string ConditionNotMet = "ConditionNotMet";
                internal const string MissingRequiredQueryParameter = "MissingRequiredQueryParameter";
                internal const string MissingRequiredHeader = "MissingRequiredHeader";
                internal const string MissingRequiredRequestBody = "MissingRequiredRequestBody";
            }

            internal static class Detail
            {
                internal static class Code
                {
                    internal const string Unsupported = "unsupported";
                    internal const string NotFound = "not-found";
                    internal const string Invalid = "invalid";
                    internal const string Missing = "missing";
                    internal const string InvalidQueryFilter = "invalid-query-filter";
                    internal const string InvalidQueryTop = "invalid-query-top";
                    internal const string InvalidQuerySkip = "invalid-query-skip";
                }

                internal static class Target
                {
                    internal const string ContentType = "content-type";
                    internal const string ApiVersion = "api-version";
                    internal const string Id = "id";
                    internal const string IfMatch = "if-match";
                    internal const string RequestBody = "request-body";
                    internal const string QueryFilter = "$filter";
                    internal const string QueryTop = "$top";
                    internal const string QuerySkip = "$skip";
                }
            }
        }
    }
}
