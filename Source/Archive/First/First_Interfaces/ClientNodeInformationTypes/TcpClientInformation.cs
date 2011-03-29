using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using First.BlackBox;

namespace First.TCP_Receiver_Emitter
{
    public class TcpClientInformation : ClientInformation
    {

        public String hostname { get; set; }

        public int port { get; set; }

        public override bool Equals(object obj)
        {
            TcpClientInformation clientInformation;

            if ((clientInformation = obj as TcpClientInformation) != null)
            {

                return clientInformation.port == port && clientInformation.hostname == hostname && clientInformation.emitterType == emitterType;

            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            return base.ToString() + "hostname: " + hostname + "; port: " + port +"; ";
        }

    }
}
