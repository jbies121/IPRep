using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace IPRep
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static async Task<AIPDBCheckRoot> AbuseIPDBCheck(string ip)
        {
            APIKeyRing mykey = new APIKeyRing();

            //set default headers for the httpclient. Might want to change this to be set by req
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", "IPRep v1.0");

            //build request uri
            string uri = "https://api.abuseipdb.com/api/v2/check?ipAddress=" + ip + "&maxAgeInDays=90&verbose=";

            //init request message obj
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, uri);
            req.Headers.Add("Key", mykey.AIPDBKey);

            //Send request async through httpclient
            HttpResponseMessage resp = await client.SendAsync(req);
            string responseBody = await resp.Content.ReadAsStringAsync();

            AIPDBCheckRoot repositories = JsonConvert.DeserializeObject<AIPDBCheckRoot>(responseBody);

            return repositories;
        }

        public static async Task Main(string[] args)
        {
            string[] query = new string[3];
            bool ask = false;

            string help = @"
IPRep v0.1.0 | .NET Core 3.1 | by jbies121
Usage: <ip address> [info] -[service]
Example: .\iprep.exe 8.8.8.8 score -AIPDB

<service>
-----
-AIPDB : AbuseIPDB

<info>
-----
score
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
                    if (args.Length > 1)
                    {
                        if (args[1].StartsWith('-'))
                        {
                            // Choose service
                            query[2] = args[1];
                        }
                        else
                        {
                            query[1] = args[1];
                        }
                        if (args.Length > 2)
                        {
                            if (args[2].StartsWith('-'))
                            {
                                // Choose service
                                query[2] = args[2];
                            }
                            else
                            {
                                query[1] = args[2];
                            }
                        }
                    }
                    if (query[1] == null)
                    {
                        query[1] = "null";
                    }
                    if (query[2] == null)
                    {
                        query[2] = "-AIPDB";
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

            if (ask == true && query[2] == "-AIPDB")
            {
                try
                {
                    //Make request and get response as deserialized json object
                    var repositories = await AbuseIPDBCheck(query[0]);

                    //Create a report. 'repositories' should be passed to a method that returns report, this is a bad place holder.
                    string[] report = { "## IP Address ##", repositories.data.ipAddress + "\n", "## Is Public? ##", repositories.data.isPublic + "\n", "## IP Version ##", repositories.data.ipVersion.ToString() + "\n", "## Is White Listed? ##", repositories.data.isWhitelisted + "\n", "## Abuse Confidence Score ##", repositories.data.abuseConfidenceScore.ToString() + "\n", "## Country Code ##", repositories.data.countryCode + "\n", "## Usage Type ##", repositories.data.usageType + "\n", "## ISP ##", repositories.data.isp + "\n", "## Domain ##", repositories.data.domain + "\n", "## Country Name ##", repositories.data.countryName + "\n", "## Total Reports ##", repositories.data.totalReports.ToString() + "\n", "## Number of Distinct Users ##", repositories.data.numDistinctUsers.ToString() + "\n", "## Last Reported At ##", repositories.data.lastReportedAt + "\n" };


                    //user supplied arguments
                    Dictionary<string, Action> actions = new Dictionary<string, Action>
                {
                    { "score", () => Console.WriteLine(repositories.data.abuseConfidenceScore) },
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
