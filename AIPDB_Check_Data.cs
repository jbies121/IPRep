using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace iprep
{
    [DataContract]
    public class AIPDB_Check_Data
    {
        [DataMember]
        public string ipAddress { get; set; }
        [DataMember]
        public bool isPublic { get; set; }
        [DataMember]
        public int ipVersion { get; set; }
        [DataMember]
        public bool isWhitelisted { get; set; }
        [DataMember]
        public int abuseConfidenceScore { get; set; }
        [DataMember]
        public string countryCode { get; set; }
        [DataMember]
        public string usageType { get; set; }
        [DataMember]
        public string isp { get; set; }
        [DataMember]
        public string domain { get; set; }
        [DataMember]
        public List<object> hostnames { get; set; }
        [DataMember]
        public string countryName { get; set; }
        [DataMember]
        public int totalReports { get; set; }
        [DataMember]
        public int numDistinctUsers { get; set; }
        [DataMember]
        public DateTime lastReportedAt { get; set; }
        [DataMember]
        public List<AIPDB_Check_Report> reports { get; set; }
    }


}