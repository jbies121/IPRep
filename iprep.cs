using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using System.Runtime.Serialization.Json;

namespace iprep
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static async Task<AIPDB_Check_Root> AbuseIPDBCheck()
        {
            HttpResponseMessage resp; //--init response message obj

            //--set default headers for the httpclient. Might want to change this to be set by req
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "IPRep v1.0");

            //--init request message obj
            var req = new HttpRequestMessage(HttpMethod.Get, "https://api.abuseipdb.com/api/v2/check?ipAddress=118.25.6.39&maxAgeInDays=90&verbose=");
            req.Headers.Add("Key", "fd1907dee8bba0772a69b2ce41990f21ff52347612f580cc35594c88ad93070ac69da9ea459752af");

            //--Send request async through httpclient
            resp = await client.SendAsync(req);
            var responseBody = await resp.Content.ReadAsStreamAsync();

            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(AIPDB_Check_Root));
            AIPDB_Check_Root repositories = (AIPDB_Check_Root)deserializer.ReadObject(responseBody);

            return repositories;
            //--Deserialize Json not working. Throwing error that responseBody string cannot be converted to List
            //var repositories = JsonSerializer.Deserialize<List<AIPDB_Check_Root>>(responseBody, options);
            //return repositories;
        }

        public static async Task Main(string[] args)
        {
            var repositories = await AbuseIPDBCheck();

            Console.WriteLine(repositories);
            
        }
    }
}
