//--From https://json2csharp.com/ based on AbuseIPDB response Json for 'check' enpoint
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

namespace IPRep
{
    public class AIPDB_Check_Root
    {
        public AIPDB_Check_Data data { get; set; }
    }


}