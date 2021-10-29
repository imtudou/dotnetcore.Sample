using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json;

namespace TestClient
{
    class Program
    {
        /// <summary>
        /// 创建客户端
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("------BEGIN------");

            // discover endpoints from metadata
            var client = new HttpClient();
            string url = "http://localhost:5000";
            var disco =  client.GetDiscoveryDocumentAsync(url).Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }


            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            }).Result;

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine(tokenResponse.Json);



            //调用API
            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);
            string apiurl = "http://localhost:5001/api/Values/Get";
            var response = apiClient.GetAsync(apiurl).Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content =  response.Content.ReadAsStringAsync();
                Console.WriteLine(JsonConvert.SerializeObject(content));
            }





            Console.WriteLine("------END------");
        }
    }
}
