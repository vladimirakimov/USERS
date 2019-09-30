using Newtonsoft.Json;
using System;

namespace ITG.Brix.Users.API.Context.Services.Responses.Models.Errors
{
    public class ResponseError : IComparable<ResponseError>
    {
        /// <summary>
        /// Gets or sets property level error code.
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public ResponseErrorBody Error { get; set; }

        public int CompareTo(ResponseError other)
        {
            return other != null ? other.Error.CompareTo(this.Error) : -1;
        }
    }
}