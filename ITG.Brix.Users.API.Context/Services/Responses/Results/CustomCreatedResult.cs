using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ITG.Brix.Users.API.Context.Services.Responses.Results
{
    public class CustomCreatedResult : CreatedResult
    {
        public CustomCreatedResult(string location, string eTagValue)
            : base(location, null)
        {
            ETagValue = eTagValue;
        }

        public string ETagValue { get; set; }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            SetHeaders(context);

            return base.ExecuteResultAsync(context);
        }

        public override void ExecuteResult(ActionContext context)
        {
            SetHeaders(context);

            base.ExecuteResult(context);
        }

        private void SetHeaders(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add("ETag", "\"" + ETagValue + "\"");
            context.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Length, Location");
        }
    }
}
