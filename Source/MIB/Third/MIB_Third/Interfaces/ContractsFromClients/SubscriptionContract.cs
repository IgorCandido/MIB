using System;
using System.Runtime.Serialization;
using Interfaces.RequestHandling;

namespace Interfaces.ContractsFromClients
{
    [Serializable]
    [DataContract]
    public class SubscriptionContract
    {

        [DataMember]
        public String ContentDescription { get; set; }

        [DataMember]
        public ClientInformationContract ClientInformation { get; set; }

      
    }
}
