using System;
using System.Runtime.Serialization;

namespace Interfaces.Contracts
{
    [Serializable]
    [DataContract]
    public class SubscriptionContract
    {

        [DataMember]
        public String Topic { get; set; }

        [DataMember]
        public ClientInformationContract ClientInformation { get; set; }

        public override string ToString()
        {
            return "topic: " + Topic + ";";
        }

    }
}
