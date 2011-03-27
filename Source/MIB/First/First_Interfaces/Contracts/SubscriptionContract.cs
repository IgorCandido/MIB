using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using First.BlackBox;
using First_Interfaces.ClientNodeInformationTypes;
using First_Interfaces.Contracts;

namespace First.Interfaces
{

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
