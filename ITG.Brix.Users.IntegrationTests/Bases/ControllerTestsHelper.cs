using ITG.Brix.Users.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace ITG.Brix.Users.IntegrationTests.Bases
{
    public static class ControllerTestsHelper
    {
        private static TestServer _server;
        public static void InitServer()
        {

            var config = new ConfigurationBuilder().AddJsonFile("settings.json", optional: false).Build();

            _server = new TestServer(new WebHostBuilder().UseConfiguration(config).UseStartup<Startup>());
        }


        public static HttpClient GetClient()
        {


            var client = _server.CreateClient();



            return client;
        }
    }
}
