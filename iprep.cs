using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
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
            HttpResponseMessage resp; //init response message obj

            //set default headers for the httpclient. Might want to change this to be set by req
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "IPRep v1.0");

            //build request uri
            string uri = "https://api.abuseipdb.com/api/v2/check?ipAddress=" + ip + "&maxAgeInDays=90&verbose=";

            //init request message obj
            var req = new HttpRequestMessage(HttpMethod.Get, uri);
            req.Headers.Add("Key", "GET_YOUR_OWN");

            //Send request async through httpclient
            resp = await client.SendAsync(req);
            var responseBody = await resp.Content.ReadAsStringAsync();

            AIPDB_Check_Root repositories = JsonConvert.DeserializeObject<AIPDB_Check_Root>(responseBody);

            return repositories;
        }

        public static async Task Main(string[] args)
        {
            string[] query = new string[2];
            bool ask = false;

            string help = @"Usage: <ip address> <info>

<info>
-----
isPublic
ipVersion
isWhitelisted
countryCode
usageType
isp
domain
hostnames
countryName
totalReports
numDistinctUsers
lastReportedAt";

            //process arguments
            if (args.Length != 0)
            {
                if (args[0] != "help")
                {
                    if (IPAddress.TryParse(args[0], out IPAddress ip))
                    {
                        query[0] = args[0];
                        ask = true;
                    }
                    else
                    {
                        Console.WriteLine("Need an IP, goober.");
                    }
                    if (args.Length == 2)
                    {
                        query[1] = args[1];
                    }
                    else
                    {
                        query[1] = "null";
                    }
                }
                else
                {
                    Console.WriteLine(help);
                }
            }
            else
            {
                Console.WriteLine(help);
            }

            if (ask)
            {
                try
                {
                    //Make request and get response as deserialized json object
                    var repositories = await AbuseIPDBCheck(query[0]);

                    //Create a report
                    string[] report = { "## IP Address ##", repositories.data.ipAddress + "\n", "## Is Public? ##", repositories.data.isPublic + "\n", "## IP Version ##", repositories.data.ipVersion.ToString() + "\n", "## Is White Listed? ##", repositories.data.isWhitelisted + "\n", "## Abuse Confidence Score ##", repositories.data.abuseConfidenceScore.ToString() + "\n", "## Country Code ##", repositories.data.countryCode + "\n", "## Usage Type ##", repositories.data.usageType + "\n", "## ISP ##", repositories.data.isp + "\n", "## Domain ##", repositories.data.domain + "\n", "## Country Name ##", repositories.data.countryName + "\n", "## Total Reports ##", repositories.data.totalReports.ToString() + "\n", "## Number of Distinct Users ##", repositories.data.numDistinctUsers.ToString() + "\n", "## Last Reported At ##", repositories.data.lastReportedAt + "\n" };
                    

                    //user supplied arguments
                    var actions = new Dictionary<string, Action>
                {
                    { "country", () => Console.WriteLine(repositories.data.countryName) },
                    { "isPublic", () => Console.WriteLine(repositories.data.isPublic) },
                    { "ipVersion", () => Console.WriteLine(repositories.data.ipVersion) },
                    { "isWhitelisted", () => Console.WriteLine(repositories.data.isWhitelisted) },
                    { "countryCode", () => Console.WriteLine(repositories.data.countryCode) },
                    { "usageType", () => Console.WriteLine(repositories.data.usageType) },
                    { "isp", () => Console.WriteLine(repositories.data.isp) },
                    { "domain", () => Console.WriteLine(repositories.data.domain) },
                    { "hostnames", () => repositories.data.hostnames.ForEach(Console.WriteLine) },
                    { "countryName", () => Console.WriteLine(repositories.data.countryName) },
                    { "totalReports", () => Console.WriteLine(repositories.data.totalReports) },
                    { "numDistinctUsers", () => Console.WriteLine(repositories.data.numDistinctUsers) },
                    { "lastReportedAt", () => Console.WriteLine(repositories.data.lastReportedAt) },
                    { "null", () => Array.ForEach(report, Console.WriteLine) }
                };

                    actions[query[1]]();
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("There was an error with your query. Check the IP address and API key and try again.");
                }
                

                
            }
        }
    }
}
