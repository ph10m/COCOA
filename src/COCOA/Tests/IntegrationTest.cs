using System;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
//using Xunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace COCOA.Tests
{
    /// <summary>
    /// Integration test base class. Sets up server and client. Uses SSL.
    /// </summary>
    [TestClass]
    public abstract class IntegrationTest
    {
        protected readonly TestServer _server;
        protected readonly HttpClient _client;

        public IntegrationTest()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<StartupTests>());
            _client = _server.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44395");
        }
    }
}
