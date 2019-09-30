using Microsoft.AspNetCore.Mvc;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models.From
{
    public class DeleteFromRoute
    {
        [FromRoute(Name = "id")]
        public string Id { get; set; }
    }
}
