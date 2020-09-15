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
        private static readonly HttpClient Client = new HttpClient();
        private static async Task<AIPDBCheckRoot> AbuseIPDBCheck(string ip)
        {
            HttpResponseMessage Resp; //init response message obj

            APIKeyRing MyKey = new APIKeyRing();

            //set default headers for the httpclient. Might want to change this to be set by req
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.Add("User-Agent", "IPRep v1.0");

            //build request uri
            string URI = "https://api.abuseipdb.com/api/v2/check?ipAddress=" + ip + "&maxAgeInDays=90&verbose=";

            //init request message obj
            var Req = new HttpRequestMessage(HttpMethod.Get, URI);
            Req.Headers.Add("Key", MyKey.AIPDBKey);

            //Send request async through httpclient
            Resp = await Client.SendAsync(Req);
            var ResponseBody = await Resp.Content.ReadAsStringAsync();

            AIPDBCheckRoot Repositories = JsonConvert.DeserializeObject<AIPDBCheckRoot>(ResponseBody);

            return Repositories;
        }

        public static async Task Main(string[] args)
        {
            string[] Query = new string[3];
            bool Ask = false;

            string Help = @"
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
                        Query[0] = args[0];
                        Ask = true;
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
                            Query[2] = args[1];
                        }
                        else
                        {
                            Query[1] = args[1];
                        }
                        if (args.Length > 2)
                        {
                            if (args[2].StartsWith('-'))
                            {
                                // Choose service
                                Query[2] = args[2];
                            }
                            else
                            {
                                Query[1] = args[2];
                            }
                        }
                    }
                    if (Query[1] == null)
                    {
                        Query[1] = "null";
                    }
                    if (Query[2] == null)
                    {
                        Query[2] = "-AIPDB";
                    }
                }
                else
                {
                    Console.WriteLine(Help);
                }
            }
            else
            {
                Console.WriteLine(Help);
            }

            if (Ask == true && Query[2] == "-AIPDB")
            {
                try
                {
                    //Make request and get response as deserialized json object
                    var Repositories = await AbuseIPDBCheck(Query[0]);

                    //Create a report. 'repositories' should be passed to a method that returns report, this is a bad place holder.
                    string[] Report = { "## IP Address ##", Repositories.data.ipAddress + "\n", "## Is Public? ##", Repositories.data.isPublic + "\n", "## IP Version ##", Repositories.data.ipVersion.ToString() + "\n", "## Is White Listed? ##", Repositories.data.isWhitelisted + "\n", "## Abuse Confidence Score ##", Repositories.data.abuseConfidenceScore.ToString() + "\n", "## Country Code ##", Repositories.data.countryCode + "\n", "## Usage Type ##", Repositories.data.usageType + "\n", "## ISP ##", Repositories.data.isp + "\n", "## Domain ##", Repositories.data.domain + "\n", "## Country Name ##", Repositories.data.countryName + "\n", "## Total Reports ##", Repositories.data.totalReports.ToString() + "\n", "## Number of Distinct Users ##", Repositories.data.numDistinctUsers.ToString() + "\n", "## Last Reported At ##", Repositories.data.lastReportedAt + "\n" };
                    

                    //user supplied arguments
                    var actions = new Dictionary<string, Action>
                {
                    { "score", () => Console.WriteLine(Repositories.data.abuseConfidenceScore) },
                    { "country", () => Console.WriteLine(Repositories.data.countryName) },
                    { "isPublic", () => Console.WriteLine(Repositories.data.isPublic) },
                    { "ipVersion", () => Console.WriteLine(Repositories.data.ipVersion) },
                    { "isWhitelisted", () => Console.WriteLine(Repositories.data.isWhitelisted) },
                    { "countryCode", () => Console.WriteLine(Repositories.data.countryCode) },
                    { "usageType", () => Console.WriteLine(Repositories.data.usageType) },
                    { "isp", () => Console.WriteLine(Repositories.data.isp) },
                    { "domain", () => Console.WriteLine(Repositories.data.domain) },
                    { "hostnames", () => Repositories.data.hostnames.ForEach(Console.WriteLine) },
                    { "countryName", () => Console.WriteLine(Repositories.data.countryName) },
                    { "totalReports", () => Console.WriteLine(Repositories.data.totalReports) },
                    { "numDistinctUsers", () => Console.WriteLine(Repositories.data.numDistinctUsers) },
                    { "lastReportedAt", () => Console.WriteLine(Repositories.data.lastReportedAt) },
                    { "null", () => Array.ForEach(Report, Console.WriteLine) }
                };

                    actions[Query[1]]();
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("There was an error with your query. Check the IP address and API key and try again.");
                }
                

                
            }
        }
    }
}
