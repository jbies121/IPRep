using System.Collections.Generic;
using System.Runtime.Serialization;

namespace iprep
{
    [DataContract]
    public class AIPDB_Check_Root
    {
        [DataMember]
        public List<AIPDB_Check_Data> data { get; set; }
    }


}