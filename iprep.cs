using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace iprep
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static async Task<AIPDB_Check_Root> AbuseIPDBCheck(string ip)
        {
            HttpResponseMessage resp; //--init response message obj

            //--set default headers for the httpclient. Might want to change this to be set by req
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "IPRep v1.0");

            //--build request uri
            string uri = "https://api.abuseipdb.com/api/v2/check?ipAddress=" + ip + "&maxAgeInDays=90&verbose=";

            //--init request message obj
            var req = new HttpRequestMessage(HttpMethod.Get, uri);
            req.Headers.Add("Key", "fba071ba17d24b73d559d5a703645d6697f0129aa3324ff50612a37108e261212a40a7d37e2288b8");

            //--Send request async through httpclient
            resp = await client.SendAsync(req);
            var responseBody = await resp.Content.ReadAsStringAsync();

            AIPDB_Check_Root repositories = JsonConvert.DeserializeObject<AIPDB_Check_Root>(responseBody);

            return repositories;
        }

        public static async Task Main(string[] args)
        {
            string cli = args[0];
            var repositories = await AbuseIPDBCheck(cli);

            Console.WriteLine(repositories.data.countryName);
            
        }
    }
}
