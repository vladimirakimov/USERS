using Microsoft.AspNetCore.Mvc;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models.From
{
    public class GetFromRoute
    {
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }
}
