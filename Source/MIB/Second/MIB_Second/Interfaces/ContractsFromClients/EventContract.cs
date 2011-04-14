using System;
using System.Runtime.Serialization;
using Interfaces.Contracts;
using Interfaces.RequestHandling;

namespace Interfaces.ContractsFromClients
{

    [Serializable]
    [DataContract]
    public class EventContract 
    {

        [DataMember]
        public String ContentDescription { get; set; }
        
        [DataMember]
        public byte[] Data { get; set; }

    }
}
