namespace IPRep
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;

    using AbuseIPDB;

    public partial class IPRep
	{
		public static string help = @"
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

		internal static async Task<bool> ProcessArgsAsync(string[] args)
		{
            string[] query = new string[3];
            bool ask = false;



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
                    Console.WriteLine(IPRep.help);
                }
            }
            else
            {
                Console.WriteLine(IPRep.help);
            }

            if (ask == true && query[2] == "-AIPDB")
            {
                try
                {
                    //Make request and get response as deserialized json object
                    var repositories = await AbuseIpdb.AbuseIPDBCheck(query[0]);

                    //Create a report. 'repositories' should be passed to a method that returns report, this is a bad place holder.
                    string[] report = { "## IP Address ##", repositories.Data.IpAddress + "\n", "## Is Public? ##", repositories.Data.IsPublic + "\n", "## IP Version ##", repositories.Data.IpVersion.ToString() + "\n", "## Is White Listed? ##", repositories.Data.IsWhitelisted + "\n", "## Abuse Confidence Score ##", repositories.Data.AbuseConfidenceScore.ToString() + "\n", "## Country Code ##", repositories.Data.CountryCode + "\n", "## Usage Type ##", repositories.Data.UsageType + "\n", "## ISP ##", repositories.Data.Isp + "\n", "## Domain ##", repositories.Data.Domain + "\n", "## Country Name ##", repositories.Data.CountryName + "\n", "## Total Reports ##", repositories.Data.TotalReports.ToString() + "\n", "## Number of Distinct Users ##", repositories.Data.NumDistinctUsers.ToString() + "\n", "## Last Reported At ##", repositories.Data.LastReportedAt + "\n" };


                    //user supplied arguments
                    Dictionary<string, Action> actions = new Dictionary<string, Action>
                {
                    { "score", () => Console.WriteLine(repositories.Data.AbuseConfidenceScore) },
                    { "country", () => Console.WriteLine(repositories.Data.CountryName) },
                    { "isPublic", () => Console.WriteLine(repositories.Data.IsPublic) },
                    { "ipVersion", () => Console.WriteLine(repositories.Data.IpVersion) },
                    { "isWhitelisted", () => Console.WriteLine(repositories.Data.IsWhitelisted) },
                    { "countryCode", () => Console.WriteLine(repositories.Data.CountryCode) },
                    { "usageType", () => Console.WriteLine(repositories.Data.UsageType) },
                    { "isp", () => Console.WriteLine(repositories.Data.Isp) },
                    { "domain", () => Console.WriteLine(repositories.Data.Domain) },
                    { "hostnames", () => Array.ForEach(repositories.Data.Hostnames, Console.WriteLine) },
                    { "countryName", () => Console.WriteLine(repositories.Data.CountryName) },
                    { "totalReports", () => Console.WriteLine(repositories.Data.TotalReports) },
                    { "numDistinctUsers", () => Console.WriteLine(repositories.Data.NumDistinctUsers) },
                    { "lastReportedAt", () => Console.WriteLine(repositories.Data.LastReportedAt) },
                    { "null", () => Array.ForEach(report, Console.WriteLine) }
                };

                    actions[query[1]]();
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("There was an error with your query. Check the IP address and API key and try again.");
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("The <info> argument supplied is not valid.");
                }



            }

            return true;
		}
	}
}

