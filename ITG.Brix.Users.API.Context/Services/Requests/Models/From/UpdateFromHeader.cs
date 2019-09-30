using Microsoft.AspNetCore.Mvc;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models.From
{
    public class UpdateFromHeader
    {
        [FromHeader(Name = "If-Match")]
        public string IfMatch { get; set; }

        [FromHeader(Name = "Content-Type")]
        public string ContentType { get; set; }
    }
}
