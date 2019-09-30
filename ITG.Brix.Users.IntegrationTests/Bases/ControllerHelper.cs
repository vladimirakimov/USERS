using ITG.Brix.Users.API.Context.Services.Requests.Models.From;
using ITG.Brix.Users.IntegrationTests.Extensions;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ITG.Brix.Users.IntegrationTests.Bases
{
    public static class ControllerHelper
    {
        public static async Task<CreateUserResult> CreateUser(string login, string password, string firstName, string lastName)
        {
            using (var client = ControllerTestsHelper.GetClient())
            {

                var apiVersion = "1.0";

                var body = new CreateFromBody()
                {
                    Login = login,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName
                };

                var jsonBody = JsonConvert.SerializeObject(body);

                var response = await client.PostAsync(string.Format("api/users?api-version={0}", apiVersion), new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                var id = response.Headers.Location.GetId();
                var eTag = response.Headers.ETag.Tag;
                var result = new CreateUserResult(id, eTag);
                return await Task.FromResult(result);
            }
        }
    }
}
