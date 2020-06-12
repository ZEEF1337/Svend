using Desktop_Klient.Controllers;
using Desktop_Klient.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Desktop_Klient.Functions
{
    class PropFunctions
    {
        public String CallRest( string URL, Array ParamArray, Method RestType)
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            RestClient client = new RestClient(ConnectionInfo.ServerIP);
            var request = new RestRequest(URL, RestType);
            foreach(RestParam Param in ParamArray)
            {
                request.AddParameter(Param.Name, Param.Value);
            }
            IRestResponse response = client.Execute(request);

            if(response.ErrorException != null || (int)response.StatusCode == 404)
            {
                return "";
            }

            var content = response.Content;
            return content;
        }
    }
}
