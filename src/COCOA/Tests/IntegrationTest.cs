using System;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Encodings.Web;

namespace COCOA.Tests
{
    /// <summary>
    /// Integration test base class. Sets up server and client. Uses SSL and JWT token authentication.
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

        /// <summary>
        /// Register & SignIn feature for client. Creates a new user and auto-signs out user if already signed in. 
        /// Uses a JWT token for authentication. JWT authentication is enabled on test server ONLY.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        protected async Task RegisterSignIn(string email)
        {
            if (_client.DefaultRequestHeaders.Authorization != null)
            {
                _client.DefaultRequestHeaders.Authorization = null;
            }

            await _client.PostAsync("/user/registeruser?email=" + UrlEncoder.Default.Encode(email) + "&name=" + email.Split('@')[0] + "&password=password", null);
            var tokenResponse = await _client.PostAsync("/user/signinusertoken?email=" + UrlEncoder.Default.Encode(email) + "&password=password", null);
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + (await tokenResponse.Content.ReadAsStringAsync()).Trim('"'));
        }
    }
}
