using Microsoft.AspNetCore.Mvc;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models.From
{
    public class UpdateFromQuery
    {
        [FromQuery(Name = "api-version")]
        public string ApiVersion { get; set; }
    }
}
