using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Interfaces.Security;

namespace Interfaces.Contracts
{

    //
    // This class is the prototype to a request object that holds operation information, this information can be
    // operation type, client credentials, etc
    //

    public enum RequestType
    {
        Event, Subscription
    }
    
    [DataContract]
    public class RequestContract
    {

        [DataMember]
        public RequestType Operation { get; set; }

    }
}
