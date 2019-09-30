using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ITG.Brix.Users.API.Context.Services.Responses.Models.Errors
{
    public class ResponseErrorBody : IComparable<ResponseErrorBody>
    {
        /// <summary>
        /// Gets or sets service-defined error code. This code serves as a sub-status for the HTTP error code specified in the response.
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets human-readable representation of the error.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the list of invalid fields send in request, in case of validation error.
        /// </summary>
        [JsonProperty(PropertyName = "details")]
        public IList<ResponseErrorField> Details { get; set; }

        public int CompareTo(ResponseErrorBody other)
        {
            if (other != null)
            {
                var compareCodeResult = other.Code.CompareTo(Code);
                if (compareCodeResult != 0)
                {
                    return compareCodeResult;
                }

                var compareMessageResult = other.Message.CompareTo(Message);
                if (compareMessageResult != 0)
                {
                    return compareMessageResult;
                }

                for (int i = 0; i < other.Details.Count; i++)
                {
                    var compareDetailResult = other.Details[i].CompareTo(Details[i]);
                    if (compareDetailResult != 0)
                    {
                        return compareDetailResult;
                    }
                }

                return 0;
            }
            return -1;
        }
    }
}
