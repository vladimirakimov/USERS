using Microsoft.AspNetCore.Mvc;

namespace ITG.Brix.Users.API.Context.Services.Requests.Models.From
{
    public class ListFromQuery
    {
        [FromQuery(Name = "api-version")]
        public string ApiVersion { get; set; }

        [FromQuery(Name = "$filter")]
        public string Filter { get; set; }

        [FromQuery(Name = "$top")]
        public string Top { get; set; }

        [FromQuery(Name = "$skip")]
        public string Skip { get; set; }
    }
}
