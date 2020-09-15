//--From https://json2csharp.com/ based on AbuseIPDB response Json for 'check' enpoint
// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

namespace IPRep
{
    public class AIPDBCheckRoot
    {
        public AIPDBCheckData data { get; set; }
    }


}