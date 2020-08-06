﻿using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            req.Headers.Add("Key", "GET_YOUR_OWN");

            //--Send request async through httpclient
            resp = await client.SendAsync(req);
            var responseBody = await resp.Content.ReadAsStringAsync();

            AIPDB_Check_Root repositories = JsonConvert.DeserializeObject<AIPDB_Check_Root>(responseBody);

            return repositories;
        }

        public static async Task Main(string[] args)
        {
            string[] query = { "8.8.8.8", "null" };
            bool ask = false;

            // process arguments
            if (args.Length != 0)
            {
                if (args[0] == "help")
                {
                    Console.WriteLine("Usage: <ip address> <info>");
                    Console.WriteLine("info: country, confidence, isp");
                } else
                {
                    query[0] = args[0];
                    ask = true;
                } if (args.Length == 2)
                {
                    query[1] = args[1];
                }
            }
            else
            {
                Console.WriteLine("Need an IP, goober.");
            }

            if (ask)
            {
                // Make request and get response as deserialized json object
                var repositories = await AbuseIPDBCheck(query[0]);

                //user supplied arguments
                var actions = new Dictionary<string, Action>
                {
                    { "country", () => Console.WriteLine(repositories.data.countryName) },
                    { "confidence", () => Console.WriteLine(repositories.data.abuseConfidenceScore) },
                    { "isp", () => Console.WriteLine(repositories.data.isp) },
                    { "null", () => Console.WriteLine(repositories.data.isp) }

                };

                actions[query[1]]();
            }
        }
    }
}
