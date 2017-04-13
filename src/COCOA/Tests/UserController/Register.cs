using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace COCOA.Tests.UserController
{
    /// <summary>
    /// Tests for user registering.
    /// </summary>
    [TestClass]
    public class Register : IntegrationTest
    {
        public Register () : base()
        {
            // Arrange RegisterDuplicate
            _client.PostAsync("/user/registeruser?email=existing%40ntnu.no&name=Test&password=password", null).Wait();
        }

        [TestMethod]
        public async Task RegisterSuccess()
        {
            // Act
            var response = await _client.PostAsync("/user/registeruser?email=test%40ntnu.no&name=Test&password=password", null);

            // Assert
            Assert.AreEqual("OK", response.StatusCode.ToString().ToUpper());
        }

        [TestMethod]
        public async Task RegisterIllegalDomain()
        {
            // Act
            var response = await _client.PostAsync("/user/registeruser?email=test%40gmail.com&name=Test&password=password", null);

            // Assert
            Assert.AreEqual("BADREQUEST", response.StatusCode.ToString().ToUpper());
        }

        [TestMethod]
        public async Task RegisterDuplicate()
        {
            // Act
            var response = await _client.PostAsync("/user/registeruser?email=existing%40ntnu.no&name=Test&password=password", null);

            // Assert
            Assert.AreEqual("BADREQUEST", response.StatusCode.ToString().ToUpper());
        }

        [TestMethod]
        public async Task RegisterName()
        {
            // Act
            await _client.PostAsync("/user/registeruser?email=name%40ntnu.no&name=Tobias&password=password", null);

            // TODO: Fix authentication by implementing token auth
            var response = await _client.GetAsync("/user/name");

            // Assert
            Assert.AreEqual("TOBIAS", (await response.Content.ReadAsStringAsync()).ToUpper());
        }
    }
}
