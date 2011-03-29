using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace First.Interfaces
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
