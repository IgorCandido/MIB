using System;
using System.Runtime.Serialization;

namespace Interfaces.Contracts
{

    [Serializable]
    [DataContract]
    public class EventContract
    {

        [DataMember]
        public string topic { get; set; }
        
        [DataMember]
        public byte[] data { get; set; }

    }
}
