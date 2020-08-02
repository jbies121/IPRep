using System;
using System.Collections.Generic;

namespace iprep
{
    //--From https://json2csharp.com/ based on AbuseIPDB response Json for 'check' enpoint
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class AIPDB_Check_Report
    {
        public DateTime reportedAt { get; set; }
        public string comment { get; set; }
        public List<int> categories { get; set; }
        public int reporterId { get; set; }
        public string reporterCountryCode { get; set; }
        public string reporterCountryName { get; set; }
    }


}