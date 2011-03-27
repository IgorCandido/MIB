using System.Runtime.Serialization;

namespace Second.Interfaces.Contracts
{
    [DataContract]
    public class EventContract
    {

        [DataMember]
        public string topic { get; set; }
        
        [DataMember]
        public object data { get; set; }

    }
}
