using Microsoft.AspNetCore.Mvc;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models.From
{
    public class DeleteFromHeader
    {
        [FromHeader(Name = "If-Match")]
        public string IfMatch { get; set; }
    }
}
