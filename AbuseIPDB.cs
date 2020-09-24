namespace AbuseIPDB
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using IPRep;
    

    public partial class AbuseIpdb
    {
        [JsonProperty("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonProperty("ipAddress")]
        public string IpAddress { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }

        [JsonProperty("ipVersion")]
        public long IpVersion { get; set; }

        [JsonProperty("isWhitelisted")]
        public string IsWhitelisted { get; set; }

        [JsonProperty("abuseConfidenceScore")]
        public long AbuseConfidenceScore { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("countryName")]
        public string CountryName { get; set; }

        [JsonProperty("usageType")]
        public string UsageType { get; set; }

        [JsonProperty("isp")]
        public string Isp { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("hostnames")]
        public string[] Hostnames { get; set; }

        [JsonProperty("totalReports")]
        public long TotalReports { get; set; }

        [JsonProperty("numDistinctUsers")]
        public long NumDistinctUsers { get; set; }

        [JsonProperty("lastReportedAt")]
        public string LastReportedAt { get; set; }

        [JsonProperty("reports")]
        public Report[] Reports { get; set; }
    }

    public partial class Report
    {
        [JsonProperty("reportedAt")]
        public string ReportedAt { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("categories")]
        public long[] Categories { get; set; }

        [JsonProperty("reporterId")]
        public long ReporterId { get; set; }

        [JsonProperty("reporterCountryCode")]
        public string ReporterCountryCode { get; set; }

        [JsonProperty("reporterCountryName")]
        public string ReporterCountryName { get; set; }
    }

    public partial class AbuseIpdb
    {
        internal static async Task<AbuseIpdb> AbuseIPDBCheck(string ip)
        {
            HttpClient client = new HttpClient();
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

            AbuseIpdb repositories = JsonConvert.DeserializeObject<AbuseIpdb>(responseBody);

            return repositories;
        }
    }
}