using Newtonsoft.Json;
using System;

namespace ITG.Brix.Users.API.Context.Services.Responses.Models.Errors
{
    public class ResponseErrorField : IComparable<ResponseErrorField>
    {
        /// <summary>
        /// Gets or sets property level error code.
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets human-readable representation of property-level error.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets property name.
        /// </summary>
        [JsonProperty(PropertyName = "target")]
        public string Target { get; set; }

        public int CompareTo(ResponseErrorField other)
        {
            if (other != null)
            {
                if (other.Code.CompareTo(this.Code) != 0)
                    return other.Code.CompareTo(this.Code);
                if (other.Message.CompareTo(this.Message) != 0)
                    return other.Message.CompareTo(this.Message);
                if (other.Target.CompareTo(this.Target) != 0)
                    return other.Target.CompareTo(this.Target);
                return 0;
            }
            return -1;
        }
    }
}
