using Desktop_Klient.Models;
using Desktop_Klient.Functions;
using System;
using Xunit;
using Newtonsoft.Json;
using RestSharp;

namespace UnitTests
{
    public class BasicTests
    {
        PropFunctions propFunc = new PropFunctions();
        [Fact]
        public void TestEndpointConnection()
        {
            string URL = "endpoints/klient/login.php";
            Method RestType = Method.POST;
            RestParam[] Params = new RestParam[] {
                new RestParam { Name = "brugernavn", Value = "123"},
                new RestParam { Name = "password", Value = "123"},
            };
            var content = propFunc.CallRest(URL, Params, RestType);
            Response data = JsonConvert.DeserializeObject<Response>(content);
            Assert.NotEmpty(content);
            Assert.Equal(0, data.Result);
        }
    }
}
