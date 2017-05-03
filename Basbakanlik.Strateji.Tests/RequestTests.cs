using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using Basbakanlik.Strateji.Entities.Models;

namespace Basbakanlik.Strateji.Tests
{
    [TestClass]
    public class RequestTests
    {
        private readonly HttpClient _client = new HttpClient();
        public RequestTests()
        {
            _client.BaseAddress = new Uri("http://localhost:48759");
        }
        [TestMethod]
        public void AddType()
        {
            var type = new RequestType
            {
                ColorClass = "bg-crimson",
                Icon = "fa fa-coffee",
                Name = "Çay",
                ListOrder = 1
            };
            var json = JsonConvert.SerializeObject(type);
            var result = _client.PostAsync("api/Request/NewType", new StringContent(json)).Result;
            Assert.IsTrue(result != null);
        }
    }
}
