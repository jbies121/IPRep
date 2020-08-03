using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace iprep
{
    //--From https://json2csharp.com/ based on AbuseIPDB response Json for 'check' enpoint
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    [DataContract]
    public class AIPDB_Check_Report
    {
        [DataMember]
        public DateTime reportedAt { get; set; }
        [DataMember]
        public string comment { get; set; }
        [DataMember]
        public List<int> categories { get; set; }
        [DataMember]
        public int reporterId { get; set; }
        [DataMember]
        public string reporterCountryCode { get; set; }
        [DataMember]
        public string reporterCountryName { get; set; }
    }


}